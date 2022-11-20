using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces;
using Domain.Models;
using Application.Services;
using System.Net;

namespace SportsBettingApp_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : Controller
    {

        private readonly ILogger<WalletController> _logger;
        private readonly WalletService _walletService;

        public WalletController(ILogger<WalletController> logger, WalletService walletService)
        {
            _logger = logger;
            _walletService = walletService;
        }


        [Route("transactions")]
        [HttpGet]
        public async Task<IEnumerable<WalletTransaction>> GetWalletTransactionsAsync()
        {
            return (await _walletService.GetAllTransactionsAsync()).OrderByDescending(t => t.Id);
        }


        [Route("deposit")]
        [HttpPost]
        public async Task<IActionResult> Deposit([FromBody] WalletTransaction walletTransactionDTO)
        {

            if (walletTransactionDTO.TransactionAmount < 5)
                return BadRequest("Minimum deposit amount is 5 €!");

            // We will just get a sum of all transactions in the database since it will provide us with a verification ledger
            var walletFinalAmount = await _walletService.GetWalletBalanceFromTransactionsAsync();

            var walletTransaction = new WalletTransaction
            {
                TransactionTimestamp = DateTime.Now,
                TransactionAmount = walletTransactionDTO.TransactionAmount,
                TransactionType = TransactionType.Deposit,
                WalletFinalAmount = walletFinalAmount + walletTransactionDTO.TransactionAmount

            };

            _walletService.InsertTransaction(walletTransaction);

            return Json(walletTransaction);

        }

        [Route("wallet-balance")]
        [HttpGet]
        public async Task<double> GetWalletFinalAmountAsync()
        {
            return await _walletService.GetWalletBalanceFromTransactionsAsync();
        }
    }
}
