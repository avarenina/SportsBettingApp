using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure.SampleData;
using System.Text.RegularExpressions;

namespace Infrastructure.Data
{
    public class DbInitializer
    {
        private readonly ILogger<DbInitializer> _logger;
        private readonly IRepository<Sport> _repositorySports;
        private readonly IRepository<BettingDay> _repositoryBettingDays;
        private readonly IRepository<BettingPair> _repositoryBettingPairs;
        private readonly IRepository<SpecialOffer> _specialOfferRepository;

        public DbInitializer(ILogger<DbInitializer> logger, IRepository<BettingDay> repositoryBettingDays, IRepository<Sport> repositorySports, IRepository<BettingPair> repositoryBettingPairs, IRepository<SpecialOffer> specialOfferRepository)
        {
            _logger = logger;
            _repositorySports = repositorySports;
            _repositoryBettingDays = repositoryBettingDays;
            _repositoryBettingPairs = repositoryBettingPairs;
            _specialOfferRepository = specialOfferRepository;
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

            if (!_specialOfferRepository.Table.Any())
            {
                InsertSpecialOfferAsync();
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
                    Icon="fa-solid fa-basketball",
                    AvailableTips = new List<Tip>() {
                        new Tip { Name = "1", Stake = 0},
                        new Tip { Name = "x", Stake = 0},
                        new Tip { Name = "2", Stake = 0},
                    }
                },
                new Sport{
                    Name="Hockey",
                    Icon="fa-solid fa-hockey-puck",
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
                    Icon="fa-solid fa-baseball",
                    AvailableTips = new List<Tip>() {
                        new Tip { Name = "1", Stake = 0},
                        new Tip { Name = "x", Stake = 0},
                        new Tip { Name = "2", Stake = 0},
                        new Tip { Name = "1x", Stake = 0},
                        new Tip { Name = "x2", Stake = 0},
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
            InsertFootball();
            InsertBasketBall();
        }

        public void InsertSpecialOfferAsync()
        {
            var bettingpairToAdd = _repositoryBettingPairs.Table.ToList();

            var specialOffers = new SpecialOffer[]
            {
                new SpecialOffer
                {
                    BettingPair = bettingpairToAdd[0],
                    SportId = bettingpairToAdd[0].Sport.Id,
                    Tips = new List<Tip>()
                    {
                         new Tip { Name = "1", Stake = 1.8 },
                        new Tip { Name = "x", Stake = 1.9 },
                        new Tip { Name = "2", Stake = 2 },
                    }
                },
                 new SpecialOffer
                {
                    BettingPair = bettingpairToAdd[1],
                    SportId = bettingpairToAdd[1].Sport.Id,
                    Tips = new List<Tip>()
                    {
                         new Tip { Name = "1", Stake = 1.15 },
                        new Tip { Name = "x", Stake = 1.9 },
                        new Tip { Name = "2", Stake = 2 },
                    }
                },
                  new SpecialOffer
                {
                    BettingPair = bettingpairToAdd[2],
                    SportId = bettingpairToAdd[2].Sport.Id,
                    Tips = new List<Tip>()
                    {
                         new Tip { Name = "1", Stake = 1.8 },
                        new Tip { Name = "x", Stake = 1.9 },
                        new Tip { Name = "2", Stake = 2 },
                    }
                }

            };

            _specialOfferRepository.Insert(specialOffers);

        }


        public void InsertFootball()
        {
            Random rand = new Random();
            var sportToAdd = _repositorySports.Table.FirstOrDefault(s => s.Name == "Football");

            var FootballData = JsonSerializer.Deserialize<FootballSampleData>(File.ReadAllText("SampleData/Football.json"));
            foreach (var round in FootballData.rounds)
            {
                foreach (var match in round.matches)
                {
                    var matchStart = DateTime.Now;
                    var minRand = 1;
                    var maxrand = 10;

                    var AddHrsOrDays = rand.Next(0, 9);

                    if (AddHrsOrDays > 6)
                    {
                        // add days
                        matchStart = matchStart.AddDays(rand.Next(1, 7));
                    }
                    else if (AddHrsOrDays > 3)
                    {
                        //Add hrs
                        matchStart = matchStart.AddHours(rand.Next(1, 24));
                    }
                    else
                    {
                        // add min
                        matchStart = matchStart.AddMinutes(rand.Next(5, 1800));
                    }

                    var newPair = new BettingPair
                    {
                        FirstOpponent = match.team1,
                        SecondOpponent = match.team2,
                        CategoryId = 0,
                        MatchStartUTC = matchStart,
                        Sport = sportToAdd,
                        Tips = new List<Tip>()
                        {
                            new Tip { Name = "1", Stake = GetRandomStake(rand,1,6) },
                            new Tip { Name = "x", Stake = GetRandomStake(rand,1,6) },
                            new Tip { Name = "2", Stake = GetRandomStake(rand,1,6) },
                            new Tip { Name = "1x", Stake = GetRandomStake(rand,1,6) },
                            new Tip { Name = "x2", Stake = GetRandomStake(rand,1,6) },
                            new Tip { Name = "12", Stake = GetRandomStake(rand,1,6) }
                        }
                    };

                    _repositoryBettingPairs.Insert(newPair);
                }
            }



        }

        public void InsertBasketBall()
        {
            Random rand = new Random();
            var sportToAdd = _repositorySports.Table.FirstOrDefault(s => s.Name == "Basketball");
            var txt = File.ReadAllText("SampleData/BasketBall.json");
            var Data = JsonSerializer.Deserialize<BasketballSampleData>(txt);
            for (var i = 0; i < Data.teams.Length / 2; i++)
            {
                var team1 = Data.teams[i].teamName;
                var team2 = Data.teams[Data.teams.Length - i - 1].teamName;

                var matchStart = DateTime.Now;
                var minRand = 1;
                var maxrand = 10;

                var AddHrsOrDays = rand.Next(0, 9);

                if (AddHrsOrDays > 6)
                {
                    // add days
                    matchStart = matchStart.AddDays(rand.Next(1, 7));
                }
                else if (AddHrsOrDays > 3)
                {
                    //Add hrs
                    matchStart = matchStart.AddHours(rand.Next(1, 24));
                }
                else
                {
                    // add min
                    matchStart = matchStart.AddMinutes(rand.Next(5, 1800));
                }

                var newPair = new BettingPair
                {
                    FirstOpponent = team1,
                    SecondOpponent = team2,
                    CategoryId = 0,
                    MatchStartUTC = matchStart,
                    Sport = sportToAdd,
                    Tips = new List<Tip>()
                        {
                            new Tip { Name = "1", Stake = GetRandomStake(rand,1,6) },
                            new Tip { Name = "x", Stake = GetRandomStake(rand,1,6) },
                            new Tip { Name = "2", Stake = GetRandomStake(rand,1,6) },

                        }
                };

                _repositoryBettingPairs.Insert(newPair);




            }



        }

        //TODO : Add some logic to it
        public double GetRandomStake(Random random, double minimum, double maximum)
        {
            return Math.Round(random.NextDouble() * (maximum - minimum) + minimum, 2);
        }
    }






}


namespace Infrastructure.SampleData
{
    public class FootballSampleData
    {
        public string name { get; set; }
        public Round[] rounds { get; set; }
    }

    public class Round
    {
        public string name { get; set; }
        public Match[] matches { get; set; }
    }

    public class Match
    {
        public string date { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public Score score { get; set; }
    }

    public class Score
    {
        public int[] ft { get; set; }
    }



    public class BasketballSampleData
    {
        public Team[] teams { get; set; }
    }

    public class Team
    {
        public int teamId { get; set; }
        public string abbreviation { get; set; }
        public string teamName { get; set; }
        public string simpleName { get; set; }
        public string location { get; set; }
    }


}
