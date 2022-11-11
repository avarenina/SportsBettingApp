

using Microsoft.EntityFrameworkCore;
using SportsBettingApp_Backend.Models;

namespace SportsBettingApp_Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<BettingPair> BettingPairs { get; set; }
        public DbSet<BettingDay> BettingDays { get; set; }
        public DbSet<Tip> Tips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sport>().ToTable("Sport");
            modelBuilder.Entity<BettingPair>().ToTable("BettingPair");
            modelBuilder.Entity<BettingDay>().ToTable("BettingDay");
            modelBuilder.Entity<Tip>().ToTable("Tip");
        }
    }
}

    

