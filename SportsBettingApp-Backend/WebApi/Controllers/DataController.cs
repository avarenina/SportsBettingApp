using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {


        private readonly IApplicationDBContext _context;
        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger, IApplicationDBContext context)
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