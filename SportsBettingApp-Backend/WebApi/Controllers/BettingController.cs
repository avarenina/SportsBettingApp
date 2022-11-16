using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces;
using Domain.Models;
using Application.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BettingController : Controller
    {
        
        private readonly ILogger<DataController> _logger;
        
        private readonly IRepository<BettingTicket> _repositoryBettingTicket;
        private readonly IRepository<Tip> _repositoryTip;
        private readonly IRepository<BettingPair> _repositoryBettingPair;
        private readonly BettingService _bettingService;

        public BettingController(ILogger<DataController> logger, 
            IRepository<BettingTicket> repositoryBettingTicket, 
            IRepository<Tip> repositoryTip, 
            IRepository<BettingPair> repositoryBettingPair,
            BettingService bettingService)
        {
            _logger = logger;
            _repositoryBettingTicket = repositoryBettingTicket;
            _repositoryTip = repositoryTip;
            _repositoryBettingPair = repositoryBettingPair;
            _bettingService = bettingService;

           
        }

        [Route("place-bet")]
        [HttpPost]
        public async Task<IActionResult> PlaceBet([FromBody] BettingTicket bettingTicket)
        {
            
            _logger.LogDebug("");

            for(var i = 0; i < bettingTicket.TicketPairs.Count(); i++)
            {
                var bettingPair = _repositoryBettingPair.Table.FirstOrDefault(bp => bp.Id == bettingTicket.TicketPairs[i].BettingPair.Id);
                var selectedTip = _repositoryTip.Table.FirstOrDefault(t => t.Id == bettingTicket.TicketPairs[i].Tip.Id);
                if (bettingPair != null && selectedTip != null)
                {
                    bettingTicket.TicketPairs[i].BettingPair = bettingPair;
                    bettingTicket.TicketPairs[i].Tip = selectedTip;
                }

            }

            bettingTicket.TicketPlacedTime = DateTime.Now;


            _bettingService.InsertTicket(bettingTicket);


            return Json("Ticket placed");
        }

    }



   

    


    



}
