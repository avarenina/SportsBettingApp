using Microsoft.AspNetCore.Mvc;
using SportsBettingApp_Backend.Models;
using SportsBettingApp_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace SportsBettingApp_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {


        private readonly DataContext _context;
        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }


        [Route("sports")]
        [HttpGet]
        public async Task<IEnumerable<Sport>> GetSportsAsync()
        {
            var sports = await _context.Sports.Include(s => s.AvailableTips).ToListAsync();
            
            return sports;
        }

        [Route("betting-days")]
        [HttpGet]
        public async Task<IEnumerable<BettingDay>> GetBettingDaysAsync()
        {
            var bettingDays = await _context.BettingDays.ToListAsync();

            return bettingDays;
        }

        [Route("betting-pairs")]
        [HttpGet]
        public async Task<IEnumerable<BettingPair>> GetBettinPairsDaysAsync()
        {
            var bettingPairs = await _context.BettingPairs.Include(s => s.Sport).Include(s => s.Tips).ToListAsync();

            return bettingPairs;
        }
    }
}