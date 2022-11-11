using Microsoft.AspNetCore.Mvc;

namespace SportsBettingApp_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        

        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger)
        {
            _logger = logger;
        }



        [HttpGet(Name = "GetBettingDays")]
        public IEnumerable<BettingDay> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new BettingDay
            {
                Date = DateTime.Now.AddDays(index),
                
            })
            .ToArray();
        }
    }
}