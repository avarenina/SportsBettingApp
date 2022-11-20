using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces;
using Domain.Models;
using Application.Services;
using System.Net;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BettingController : Controller
    {
        
        private readonly ILogger<DataController> _logger;
        private readonly BettingService _bettingService;
        private readonly WalletService _walletService;

        public BettingController(ILogger<DataController> logger,
            BettingService bettingService,
            WalletService walletService)
        {
            _logger = logger;
            _bettingService = bettingService;
            _walletService = walletService;
        }

        [Route("place-bet")]
        [HttpPost]
        public async Task<IActionResult> PlaceBetAsync([FromBody] BettingTicket bettingTickeDTO)
        {            
            try
            {
                var bettingTicket = _bettingService.ValidateAndConstructBettingTicket(bettingTickeDTO);

                var walletBalance = await _walletService.GetWalletBalanceFromTransactionsAsync();

                if (bettingTicket.BetAmount > walletBalance)
                    return BadRequest("Wallet balance is too low for the bet! Please add funds to your wallet!");

                // create a transaction for this ticket
                var transaction = new WalletTransaction
                {
                    TransactionTimestamp = DateTime.Now,
                    TransactionAmount = (-bettingTicket.BetAmount),
                    TransactionType = TransactionType.Bet,
                    WalletFinalAmount = walletBalance - bettingTicket.BetAmount
                };

                bettingTicket.WalletTransaction = transaction;

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
