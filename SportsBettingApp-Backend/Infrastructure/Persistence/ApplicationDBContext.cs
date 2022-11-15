using Application.Common.Interfaces;
using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDBContext : DbContext, IApplicationDBContext
    {
        #region Ctor
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
         : base(options)
        {
        }
        #endregion
        #region DbSet
        public DbSet<Sport> Sports { get; set; }
        public DbSet<BettingPair> BettingPairs { get; set; }
        public DbSet<BettingDay> BettingDays { get; set; }
        public DbSet<Tip> Tips { get; set; }
        public DbSet<BettingTicket> BettingTickets { get; set; }
        public DbSet<TicketPair> TicketPairs { get; set; }
        #endregion
        #region Methods
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

       
        

        #endregion

    }
}