using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class DbInitializer
    {
        private readonly ILogger<DbInitializer> _logger;
        private readonly IRepository<Sport> _repositorySports;
        private readonly IRepository<BettingDay> _repositoryBettingDays;
        private readonly IRepository<BettingPair> _repositoryBettingPairs;

        public DbInitializer(ILogger<DbInitializer> logger, IRepository<BettingDay> repositoryBettingDays, IRepository<Sport> repositorySports, IRepository<BettingPair> repositoryBettingPairs)
        {
            _logger = logger;
            _repositorySports = repositorySports;
            _repositoryBettingDays = repositoryBettingDays;
            _repositoryBettingPairs = repositoryBettingPairs;
        }

        public void Initialize()
        {

            if (!_repositorySports.Table.Any())
            {
                InsertSportsAsync();
            }

            if (!_repositoryBettingDays.Table.Any())
            {
                InsertBettingDaysAsync();
            }

            if (!_repositoryBettingPairs.Table.Any())
            {
                InsertBettingPairsAsync();
            }

        }

        public void InsertSportsAsync()
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

            _repositorySports.Insert(sports);

        }

        public void InsertBettingDaysAsync()
        {
            var currentTime = DateTime.Now;

            for (int i = 0; i < 7; i++)
            {

                var bettingDay = new BettingDay { Date = currentTime.AddDays(i).Date, Label = currentTime.AddDays(i).Date.ToString("ddd, dd"), QueryStringId = currentTime.AddDays(i).Day.ToString() };
               _repositoryBettingDays.Insert(bettingDay);
            }

            
        }

        public void InsertBettingPairsAsync()
        {
            var sportToAdd = _repositorySports.Table.FirstOrDefault(s => s.Name == "Football");

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

            _repositoryBettingPairs.Insert(bettingPairs);   
        }
    }
}
