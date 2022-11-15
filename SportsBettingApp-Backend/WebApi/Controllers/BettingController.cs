using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces;
using Domain.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BettingController : Controller
    {
        private readonly IApplicationDBContext _context;
        private readonly ILogger<DataController> _logger;

        public BettingController(ILogger<DataController> logger, IApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("place-bet")]
        [HttpPost]
        public async Task<IActionResult> PlaceBet([FromBody] BettingTicket bettingTicket)
        {
            _logger.LogDebug("");
            _context.BettingTickets.Add(bettingTicket);
        
            await _context.SaveChangesAsync();


            return Json("test");
        }

    }



   

    


    



}
