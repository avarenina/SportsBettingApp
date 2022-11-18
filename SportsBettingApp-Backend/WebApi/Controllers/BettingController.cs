using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces;
using Domain.Models;
using Application.Services;
using System.Net;

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
        public IActionResult PlaceBet([FromBody] BettingTicket bettingTickeDTO)
        {            
            try
            {
                var bettingTicket = _bettingService.ValidateAndConstructBettingTicket(bettingTickeDTO);



                _bettingService.InsertBettingTicket(bettingTicket);

                return Json(bettingTicket);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return BadRequest("Unable to place your bet! Please try again!");
            }
           
        }

        [Route("betting-tickets")]
        [HttpGet]
        public IActionResult GetBettingTickets()
        {
            return Json(_bettingService.GetAllBettingTickets().OrderByDescending(bp => bp.Id));
        }

    }



   

    


    



}
