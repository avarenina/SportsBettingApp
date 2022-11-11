using SportsBettingApp_Backend.Models;
using System;
using System.Diagnostics;
using System.Linq;
namespace SportsBettingApp_Backend.Data
{
    public class DbInitializer
    {

        private readonly DataContext _dataContext;

        public DbInitializer(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Initialize()
        {
            _dataContext.Database.EnsureCreated();

            // Look for any students.
            if (_dataContext.Sports.Any())
            {
                return;   // DB has been seeded
            }

            var sports = new Sport[]
            {
                new Sport{
                    Name="Football", 
                    Icon="",
                    AvailableTips = new List<Tip>() {
                        new Tip { Name = "1", Stake = 0}, 
                        new Tip { Name = "x", Stake = 0 }, 
                        new Tip { Name = "2", Stake = 0 },
                        new Tip { Name = "1x", Stake = 0}
                     } 
                }
            };
            foreach (Sport s in sports)
            {
                _dataContext.Sports.Add(s);
            }
            _dataContext.SaveChanges();

            
        }
    }
}
