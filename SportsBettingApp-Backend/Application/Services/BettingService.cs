using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;


namespace Application.Services
{
    public class BettingService
    {
        private readonly IRepository<BettingTicket> _repositoryBettingTickets;
        private readonly IRepository<Tip> _repositoryTip;
        private readonly IRepository<BettingPair> _repositoryBettingPair;
        private readonly IRepository<TicketPair> _ticketPairRepository;
        private readonly ILogger<DataService> _logger;
       

        public BettingService(ILogger<DataService> logger, 
            IRepository<BettingTicket> repositoryBettingTickets,
            IRepository<Tip> repositoryTip,
            IRepository<BettingPair> repositoryBettingPair,
            IRepository<TicketPair> ticketPairRepository)
        {
            _logger = logger;
            _repositoryBettingTickets = repositoryBettingTickets;
            _repositoryTip = repositoryTip;
            _repositoryBettingPair = repositoryBettingPair;
            _ticketPairRepository = ticketPairRepository;
        }


        public void InsertBettingTicket(BettingTicket bettingTicket)
        {
            if (bettingTicket == null)
                throw new ArgumentNullException(nameof(bettingTicket));

            _repositoryBettingTickets.Insert(bettingTicket);
        }

        public BettingTicket GetBettingTicketById(int id)
        {
            if (id == 0)
                return null;

            return _repositoryBettingTickets.GetById(id);
        }

        public IEnumerable<BettingTicket> GetAllBettingTickets()
        {
            return _repositoryBettingTickets.Table.Include(bt => bt.TicketPairs)
                .ThenInclude(x => x.BettingPair)
                .Include(y => y.TicketPairs).ThenInclude(z => z.Tip).IgnoreAutoIncludes()
                .ToList();
        }

        public BettingTicket ValidateAndConstructBettingTicket(BettingTicket bettingTicketDTO)
        {
            if (bettingTicketDTO == null)
                throw new ArgumentNullException(nameof(bettingTicketDTO));

            if (!bettingTicketDTO.TicketPairs.Any())
                throw new Exception("Betting ticket has to have at least one betting pair");

            // General properties
            var bettingTicket = new BettingTicket()
            {
                TicketPairs = new List<TicketPair>(),
                BetAmount = bettingTicketDTO.BetAmount,
                TicketPlacedTime = DateTime.Now,
                IsWinningTicket = false,
                IsCompleted = false,
            };

            // Assemble the pairs
            foreach (var ticketPair in bettingTicketDTO.TicketPairs)
            {
                var bettingPair = _repositoryBettingPair.Table.FirstOrDefault(bp => bp.Id ==ticketPair.BettingPair.Id);
                var selectedTip = _repositoryTip.Table.FirstOrDefault(t => t.Id == ticketPair.Tip.Id);
                if (bettingPair != null && selectedTip != null)
                {
                    // Means that the betting pair has already started! This should not happen because we filter betting pairs on the frontend
                    if (bettingPair.MatchStartUTC <= DateTime.Now)
                        throw new Exception("One or more betting pairs have started playing!");
                    
                    bettingTicket.TicketPairs.Add(new TicketPair { BettingPair = bettingPair, Tip = selectedTip });
                }
                else
                {
                    throw new Exception("Betting ticket contains some invalid betting pairs and tips!");
                }

            }

            bettingTicket.TotalStake = CalculateTotalStake(bettingTicket);
            bettingTicket.ManipulationCost = bettingTicket.BetAmount * 0.05; // 5% of manipultion fee
            bettingTicket.BetAmountFinal = bettingTicket.BetAmount - bettingTicket.ManipulationCost; // This is the amount that will be multiplied with total stake
            bettingTicket.WinAmount = bettingTicket.TotalStake * bettingTicket.BetAmountFinal; // Amount for win before taxes
            bettingTicket.TaxAmount = CalculateTax(bettingTicket.WinAmount, bettingTicket.BetAmountFinal);
            bettingTicket.PayoutAmount = bettingTicket.WinAmount - bettingTicket.TaxAmount;

            return bettingTicket;
        }


        #region "Utils"

        private double CalculateTotalStake(BettingTicket bettingTicket)
        {
            if (bettingTicket == null)
                throw new ArgumentNullException(nameof(bettingTicket));

            var totalStake = 1d; // this is so that we can start multiplcation right away

            foreach (var pair in bettingTicket.TicketPairs)
            {
                totalStake *= pair.Tip.Stake;
            }


            return totalStake;
        }

        private double CalculateTax(double winAmount, double betAmount)
        {

            // Apply tax classes here
            /*
                0       - 10.000    kn	10 % -- class 1
                10.000  - 30.000    kn	15 % -- class 2
                30.000  - 500.000   kn	20 % -- class 3
                500.000 - inf       kn  30 % -- class 4
            */

            var taxBase = winAmount - betAmount;
                        
            var taxClass1Amount = 0d;
            var taxClass2Amount = 0d;
            var taxClass3Amount = 0d;
            var taxClass4Amount = 0d;

            if (taxBase > 500000)
            {
                taxClass4Amount = (taxBase - 500000) * 0.3;
                taxClass3Amount = (500000 - 30000) * 0.2;
                taxClass2Amount = (30000 - 10000) * 0.15;
                taxClass1Amount = 10000 * 0.1;
            }
            else if (taxBase > 30000)
            {
                taxClass3Amount = (taxBase - 30000) * 0.2;
                taxClass2Amount = (30000 - 10000) * 0.15;
                taxClass1Amount = 10000 * 0.1;
            }
            else if (taxBase > 10000)
            {
                taxClass2Amount = (taxBase - 10000) * 0.15;
                taxClass1Amount = 10000 * 0.1;
            }
            else
            {
                taxClass1Amount = taxBase * 0.1;
            }

            return taxClass1Amount + taxClass2Amount + taxClass3Amount + taxClass4Amount;

        }

        #endregion
    }
}
