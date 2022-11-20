using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;


namespace Application.Services
{
    public class BettingService
    {
        private readonly IRepository<BettingTicket> _bettingTicketRepository;
        private readonly IRepository<Tip> _tipRepository;
        private readonly IRepository<BettingPair> _bettingPairRepository;
        private readonly IRepository<TicketPair> _ticketPairRepository;
        private readonly ILogger<DataService> _logger;
        private readonly IRepository<SpecialOffer> _specialOfferRepository;

        public BettingService(ILogger<DataService> logger, 
            IRepository<BettingTicket> bettingTicketRepository,
            IRepository<Tip> tipRepository,
            IRepository<BettingPair> bettingPairRepository,
            IRepository<TicketPair> ticketPairRepository,
            IRepository<SpecialOffer> specialOfferRepository)
        {
            _logger = logger;
            _bettingTicketRepository = bettingTicketRepository;
            _tipRepository = tipRepository;
            _bettingPairRepository = bettingPairRepository;
            _ticketPairRepository = ticketPairRepository;
            _specialOfferRepository = specialOfferRepository;
        }


        public void InsertBettingTicket(BettingTicket bettingTicket)
        {
            if (bettingTicket == null)
                throw new ArgumentNullException(nameof(bettingTicket));

            _bettingTicketRepository.Insert(bettingTicket);
        }

        public BettingTicket GetBettingTicketById(int id)
        {
            if (id == 0)
                return null;

            return _bettingTicketRepository.GetById(id);
        }

        public IEnumerable<BettingTicket> GetAllBettingTickets()
        {
            return _bettingTicketRepository.Table.Include(bt => bt.TicketPairs)
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

            var specialOfferPairs = new List<BettingPair>();

            // Assemble the pairs
            foreach (var ticketPair in bettingTicketDTO.TicketPairs)
            {
                var bettingPair = _bettingPairRepository.Table.FirstOrDefault(bp => bp.Id ==ticketPair.BettingPair.Id);
                var selectedTip = _tipRepository.Table.FirstOrDefault(t => t.Id == ticketPair.Tip.Id);

                if (bettingPair != null && selectedTip != null)
                {
                    // Means that the betting pair has already started! This should not happen because we filter betting pairs on the frontend
                    if (bettingPair.MatchStartUTC <= DateTime.Now)
                        throw new Exception("One or more betting pairs have started playing!");
                    
                    if(IsTipFromSpecialOffer(bettingPair, selectedTip))
                    {
                        specialOfferPairs.Add(bettingPair);
                    }

                    bettingTicket.TicketPairs.Add(new TicketPair { BettingPair = bettingPair, Tip = selectedTip });
                }
                else
                {


                    throw new Exception("Betting ticket contains some invalid betting pairs and tips!");
                }

            }

            if(specialOfferPairs.Count > 1)
            {
                throw new Exception("Ticket can contain only one specail pair!");
            }
            else if(specialOfferPairs.Count == 1)
            {
                var tipsWithSmallStake = bettingTicket.TicketPairs.Where(tp => tp.Tip.Stake < 1.1);
                if(tipsWithSmallStake.Any())
                {
                    throw new Exception("Ticket can contain only tips with stake >= 1.1!");
                }
                else if(bettingTicket.TicketPairs.Count < 5)
                {
                    throw new Exception("Ticket that has special offer has to have at least 4 other pairs!");
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

        private bool IsTipFromSpecialOffer(BettingPair bettingPair, Tip tip)
        {
            var specialOffer = _specialOfferRepository.Table.FirstOrDefault(so => so.BettingPair.Id == bettingPair.Id && so.Tips.Any(t => t.Id == tip.Id));
            if(specialOffer == null)
            {
                return false;
            }

            return true;
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
