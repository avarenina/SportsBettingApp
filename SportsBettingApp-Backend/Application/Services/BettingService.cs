using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class BettingService
    {
        private readonly IRepository<BettingTicket> _repositoryBettingTickets;
        private readonly ILogger<DataService> _logger;
       

        public BettingService(ILogger<DataService> logger, IRepository<BettingTicket> repositoryBettingTickets)
        {
            _logger = logger;
            _repositoryBettingTickets = repositoryBettingTickets;
        }


       public void InsertTicket(BettingTicket ticket)
        {
            _repositoryBettingTickets.Insert(ticket);
        }



    }
}
