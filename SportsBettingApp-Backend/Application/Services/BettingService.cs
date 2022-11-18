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

            // General properties
            var bettingTicket = new BettingTicket()
            {
                TicketPairs = new List<TicketPair>(),
                BetAmount = bettingTicketDTO.BetAmount,
                TicketPlacedTime = DateTime.Now,
                IsWinningTicket = false
            };

            // Assemble the pairs
            foreach (var ticketPair in bettingTicketDTO.TicketPairs)
            {
                var bettingPair = _repositoryBettingPair.Table.FirstOrDefault(bp => bp.Id ==ticketPair.BettingPair.Id);
                var selectedTip = _repositoryTip.Table.FirstOrDefault(t => t.Id == ticketPair.Tip.Id);
                if (bettingPair != null && selectedTip != null)
                {
                    bettingTicket.TicketPairs.Add(new TicketPair { BettingPair = bettingPair, Tip = selectedTip });
                }
                else
                {
                    throw new Exception("Betting ticket contains some invalid betting pairs and tips!");
                }

            }


            return bettingTicket;


        }

    }
}
