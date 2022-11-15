using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDBContext
    {
        public DbSet<Sport> Sports { get; set; }
        public DbSet<BettingPair> BettingPairs { get; set; }
        public DbSet<BettingDay> BettingDays { get; set; }
        public DbSet<Tip> Tips { get; set; }
        public DbSet<BettingTicket> BettingTickets { get; set; }
        public DbSet<TicketPair> TicketPairs { get; set; }


        Task<int> SaveChangesAsync();
       

    }
}