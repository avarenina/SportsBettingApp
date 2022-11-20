using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Application.Services
{
    public class ResultsService : IHostedService, IDisposable
    {
        private readonly ILogger<ResultsService> _logger;
        private readonly IRepository<BettingPairResult> _bettingPairResultRepository;
        private readonly IRepository<BettingPair> _bettingPairRepository;
        private readonly IRepository<BettingTicket> _bettingTicketRepository;
        private readonly BettingService _bettingService;
        private readonly DataService _dataService;
        private readonly WalletService _walletService;

        private Timer? _timer = null;

        public ResultsService(ILogger<ResultsService> logger,
            IRepository<BettingPair> bettingPairRepository, 
            IRepository<BettingTicket> bettingTicketRepository, 
            IRepository<BettingPairResult> bettingPairResultRepository, 
            BettingService bettingService, 
            DataService dataService,
            WalletService walletService)
        {
            _logger = logger;
            _bettingPairRepository = bettingPairRepository;
            _bettingTicketRepository = bettingTicketRepository;
            _bettingPairResultRepository = bettingPairResultRepository;
            _bettingService = bettingService;
            _dataService = dataService;
            _walletService = walletService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            Random rand = new Random();
            
            var bettingPairsForProcessing = _bettingPairRepository.Table.Where(bp => !_bettingPairResultRepository.Table.Any(r => r.BettingPair.Id == bp.Id) && bp.MatchStartUTC < DateTime.Now);
            foreach(var bettingPair in bettingPairsForProcessing)
            {
                var bettingPairResult = new BettingPairResult
                {
                    BettingPair = bettingPair,
                    FirstOpponentScore = rand.Next(0, 1),
                    SecondOpponentScore = rand.Next(0, 1)
                };

                // Mark tips as 
                if(bettingPairResult.FirstOpponentScore == bettingPairResult.SecondOpponentScore)
                {
                    bettingPairResult.WinningTip = "x";
                }
                else if(bettingPairResult.FirstOpponentScore < bettingPairResult.SecondOpponentScore)
                {
                    bettingPairResult.WinningTip = "2";
                }
                else
                {
                    bettingPairResult.WinningTip = "1";
                }

                _bettingPairResultRepository.Insert(bettingPairResult);
            }


            // Now process tickets 
            var bettingTickets = _bettingTicketRepository.Table.Include(bt => bt.TicketPairs).ThenInclude(x => x.BettingPair).Include(y => y.TicketPairs).ThenInclude(z => z.Tip).Where(bt => !bt.IsCompleted);
            foreach(var ticket in bettingTickets)
            {
                var results = _bettingPairResultRepository.Table.Include(bpr => bpr.BettingPair);

                var finishedPairs = ticket.TicketPairs.Where(tp => results.Any(r => r.BettingPair.Id == tp.BettingPair.Id));

                if(finishedPairs.Any() && finishedPairs.Count() == ticket.TicketPairs.Count)
                {
                    bool IsWinningTicket = true;
                                        // all of them are finished, compare with the tips
                    foreach(var finishedPair in finishedPairs)
                    {
                        var pairResult = _bettingPairResultRepository.Table.FirstOrDefault(pr => pr.BettingPair.Id == finishedPair.BettingPair.Id);
                        if(pairResult != null)
                        {
                            if(finishedPair.Tip.Name.ToLower().Contains(pairResult.WinningTip))
                            {
                                // this pair is win
                               
                            }
                            else
                            {
                                // not a win
                                IsWinningTicket = false;
                            }
                        }

                    }

                    ticket.IsCompleted = true;

                    if(IsWinningTicket)
                    {
                        var walletBalance = _walletService.GetWalletBalanceFromTransactions();


                        // update ticket here
                        ticket.IsWinningTicket = IsWinningTicket;

                        // Insert Transaction
                        // create a transaction for this ticket
                        var transaction = new WalletTransaction
                        {
                            TransactionTimestamp = DateTime.Now,
                            TransactionAmount = ticket.PayoutAmount,
                            TransactionType = TransactionType.Win,
                            WalletFinalAmount = walletBalance + ticket.PayoutAmount
                        };


                        _walletService.InsertTransaction(transaction);




                    }
                   
                    _bettingTicketRepository.Update(ticket);
                }


            }




            _logger.LogInformation(
                "Timed Hosted Service is working.");
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }



        
    }
}
