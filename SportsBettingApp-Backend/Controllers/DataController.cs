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


        [Route("GetSports")]
        [HttpGet]
        public async Task<IEnumerable<Sport>> GetSportsAsync()
        {
            var sports = await _context.Sports.Include(s => s.AvailableTips).ToListAsync();
            
            return sports;
        }
    }
}