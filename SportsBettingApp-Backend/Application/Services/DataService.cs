using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class DataService
    {
        
        private readonly ILogger<DataService> _logger;
        private readonly IRepository<Sport> _sportRepository;
        private readonly IRepository<BettingDay> _bettingDayRepository;
        private readonly IRepository<BettingPair> _bettingPairRepository;
        private readonly IRepository<SpecialOffer> _specialOfferRepository;

        public DataService(ILogger<DataService> logger, IRepository<BettingDay> bettingDayRepository, IRepository<Sport> sportRepository, IRepository<BettingPair> bettingPairRepository, IRepository<SpecialOffer> specialOfferRepository)
        {
            _logger = logger;
            _sportRepository = sportRepository;
            _bettingDayRepository = bettingDayRepository;
            _bettingPairRepository = bettingPairRepository;
            _specialOfferRepository = specialOfferRepository;
        }



        public async Task<IEnumerable<Sport>> GetSportsAsync()
        {
            var sports = await _sportRepository.Table.Include(s => s.AvailableTips).ToListAsync();

            return sports;
        }

        public async Task<IEnumerable<BettingDay>> GetBettingDaysAsync()
        {
            var bettingDays = await _bettingDayRepository.Table.ToListAsync();

            return bettingDays;
        }

        public async Task<IEnumerable<BettingPair>> GetBettingPairsAsync()
        {
            var bettingPairs = await _bettingPairRepository.Table.Include(s => s.Sport).Include(s => s.Tips).ToListAsync();

            return bettingPairs;
        }

        public async Task<IEnumerable<SpecialOffer>> GetSpecialOfferAsync()
        {
            var specialOffers = await _specialOfferRepository.Table.Include(bp => bp.BettingPair).Include(so => so.Tips).ToListAsync();

            return specialOffers;
        }
    }
}
