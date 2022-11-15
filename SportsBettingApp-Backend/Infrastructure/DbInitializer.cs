using Application.Common.Interfaces;
using Domain.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace Infrastructure.Data
{
    public class DbInitializer
    {

        private readonly IApplicationDBContext _dataContext;

        public DbInitializer(IApplicationDBContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task InitializeAsync()
        {
           

            if (!_dataContext.Sports.Any())
            {
                await InsertSportsAsync();
            }

            if (!_dataContext.BettingDays.Any())
            {
                await InsertBettingDaysAsync();
            }

            if (!_dataContext.BettingPairs.Any())
            {
                await InsertBettingPairsAsync();
            }



        }

        public async Task InsertSportsAsync()
        {
            var sports = new Sport[]
            {
                new Sport{
                    Name="Football",
                    Icon="fa-solid fa-futbol",
                    AvailableTips = new List<Tip>() {
                        new Tip { Name = "1", Stake = 0},
                        new Tip { Name = "x", Stake = 0},
                        new Tip { Name = "2", Stake = 0},
                        new Tip { Name = "1x", Stake = 0},
                        new Tip { Name = "x2", Stake = 0},
                        new Tip { Name = "12", Stake = 0}
                    }
                },
                new Sport{
                    Name="Basketball",
                    Icon="fa-solid fa-futbol",
                    AvailableTips = new List<Tip>() {
                        new Tip { Name = "1", Stake = 0},
                        new Tip { Name = "x", Stake = 0},
                        new Tip { Name = "2", Stake = 0},
                        new Tip { Name = "1x", Stake = 0},
                        new Tip { Name = "x2", Stake = 0}
                    }
                },
                new Sport{
                    Name="Hockey",
                    Icon="fa-solid fa-futbol",
                    AvailableTips = new List<Tip>() {
                        new Tip { Name = "1", Stake = 0},
                        new Tip { Name = "x", Stake = 0},
                        new Tip { Name = "2", Stake = 0},
                        new Tip { Name = "1x", Stake = 0},
                        new Tip { Name = "x2", Stake = 0},
                        new Tip { Name = "12", Stake = 0}
                    }
                },
                new Sport{
                    Name="Handball",
                    Icon="fa-solid fa-futbol",
                    AvailableTips = new List<Tip>() {
                        new Tip { Name = "1", Stake = 0},
                        new Tip { Name = "x", Stake = 0},
                        new Tip { Name = "2", Stake = 0},
                        new Tip { Name = "1x", Stake = 0},
                        new Tip { Name = "x2", Stake = 0},
                        new Tip { Name = "12", Stake = 0}
                    }
                },
                new Sport{
                    Name="Tennis",
                    Icon="fa-solid fa-futbol",
                    AvailableTips = new List<Tip>() {
                        new Tip { Name = "1", Stake = 0},
                        new Tip { Name = "2", Stake = 0}
                    }
                },
                new Sport{
                    Name="Baseball",
                    Icon="fa-solid fa-futbol",
                    AvailableTips = new List<Tip>() {
                        new Tip { Name = "1", Stake = 0},
                        new Tip { Name = "x", Stake = 0},
                        new Tip { Name = "2", Stake = 0},
                        new Tip { Name = "1x", Stake = 0},
                        new Tip { Name = "x2", Stake = 0},
                        new Tip { Name = "12", Stake = 0}
                    }
                },
                new Sport{
                    Name="Rugby",
                    Icon="fa-solid fa-futbol",
                    AvailableTips = new List<Tip>() {
                        new Tip { Name = "1", Stake = 0},
                        new Tip { Name = "x", Stake = 0},
                        new Tip { Name = "2", Stake = 0},
                        new Tip { Name = "1x", Stake = 0},
                        new Tip { Name = "x2", Stake = 0}
                    }
                },
                new Sport{
                    Name="Snooker",
                    Icon="fa-solid fa-futbol",
                    AvailableTips = new List<Tip>() {
                        new Tip { Name = "1", Stake = 0},
                        new Tip { Name = "2", Stake = 0}
                    }
                }
            };
            foreach (Sport s in sports)
            {
                _dataContext.Sports.Add(s);
            }
           await _dataContext.SaveChangesAsync();
        }

        public async Task InsertBettingDaysAsync()
        {
            var currentTime = DateTime.Now;

            for (int i = 0; i < 7; i++)
            {

                var bettingDay = new BettingDay { Date = currentTime.AddDays(i).Date, Label = currentTime.AddDays(i).Date.ToString("ddd, dd"), QueryStringId = currentTime.AddDays(i).Day.ToString() };
                _dataContext.BettingDays.Add(bettingDay);
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task InsertBettingPairsAsync()
        {
            var sportToAdd = _dataContext.Sports.FirstOrDefault(s => s.Name == "Football");

            var bettingPairs = new BettingPair[]
            {
                new BettingPair
                {
                    FirstOpponent = "Hajduk",
                    SecondOpponent = "Dinamo",
                    CategoryId = 0,
                    MatchStartUTC = DateTime.Now.AddMinutes(30),
                    Sport = sportToAdd,
                    Tips = new List<Tip>()
                    {
                        new Tip { Name = "1", Stake = 1.5 },
                        new Tip { Name = "x", Stake = 1.5 },
                        new Tip { Name = "2", Stake = 1.5 },
                        new Tip { Name = "1x", Stake = 1.5 },
                        new Tip { Name = "x2", Stake = 1.5 },
                        new Tip { Name = "12", Stake = 1.5 }
                    }
                },
                new BettingPair
                {
                    FirstOpponent = "Rijeka",
                    SecondOpponent = "Osjek",
                    CategoryId = 0,
                    MatchStartUTC = DateTime.Now.AddDays(1),
                    Sport = sportToAdd,
                    Tips = new List<Tip>()
                    {
                        new Tip { Name = "1", Stake = 1.5 },
                        new Tip { Name = "x", Stake = 1.5 },
                        new Tip { Name = "2", Stake = 1.5 },
                        new Tip { Name = "1x", Stake = 1.5 },
                        new Tip { Name = "x2", Stake = 1.5 },
                        new Tip { Name = "12", Stake = 1.5 }
                    }
                },
                new BettingPair
                {
                    FirstOpponent = "Varazdin",
                    SecondOpponent = "Splir",
                    CategoryId = 0,
                    MatchStartUTC = DateTime.Now.AddDays(1),
                    Sport = sportToAdd,
                    Tips = new List<Tip>()
                    {
                        new Tip { Name = "1", Stake = 1.5 },
                        new Tip { Name = "x", Stake = 1.5 },
                        new Tip { Name = "2", Stake = 1.5 },
                        new Tip { Name = "1x", Stake = 1.5 },
                        new Tip { Name = "x2", Stake = 1.5 },
                        new Tip { Name = "12", Stake = 1.5 }
                    }
                },

            };

            foreach(var bettingPair in bettingPairs)
            {
                _dataContext.BettingPairs.Add(bettingPair);
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}
