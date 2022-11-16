using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class DataService
    {
        
        private readonly ILogger<DataService> _logger;
        private readonly IRepository<Sport> _repositorySport;
        private readonly IRepository<BettingDay> _repositoryBettingDay;
        private readonly IRepository<BettingPair> _repositoryBettingPair;

        public DataService(ILogger<DataService> logger, IRepository<BettingDay> repositoryBettingDay, IRepository<Sport> repositorySport, IRepository<BettingPair> repositoryBettingPair)
        {
            _logger = logger;
            _repositorySport = repositorySport;
            _repositoryBettingDay = repositoryBettingDay;
            _repositoryBettingPair = repositoryBettingPair;
        }



        public async Task<IEnumerable<Sport>> GetSportsAsync()
        {
            var sports = await _repositorySport.Table.Include(s => s.AvailableTips).ToListAsync();

            return sports;
        }

        public async Task<IEnumerable<BettingDay>> GetBettingDaysAsync()
        {
            var bettingDays = await _repositoryBettingDay.Table.ToListAsync();

            return bettingDays;
        }

        public async Task<IEnumerable<BettingPair>> GetBettingPairsAsync()
        {
            var bettingPairs = await _repositoryBettingPair.Table.Include(s => s.Sport).Include(s => s.Tips).ToListAsync();

            return bettingPairs;
        }
    }
}
