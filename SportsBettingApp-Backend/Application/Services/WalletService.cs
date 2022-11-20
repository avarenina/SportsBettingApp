using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class WalletService
    {

        private readonly ILogger<WalletService> _logger;
        private readonly IRepository<WalletTransaction> _walletTransactionRepository;

        public WalletService(ILogger<WalletService> logger, IRepository<WalletTransaction> walletTransactionrepository)
        {
            _logger = logger;
            _walletTransactionRepository = walletTransactionrepository;
        }


        public void InsertTransaction(WalletTransaction walletTransaction)
        {
            if(walletTransaction == null)
                throw new ArgumentNullException(nameof(walletTransaction));


            _walletTransactionRepository.Insert(walletTransaction);

        }

        public async Task<IEnumerable<WalletTransaction>> GetAllTransactionsAsync()
        {
            var transactions = await _walletTransactionRepository.Table.ToListAsync();

            return transactions;
        }

        public async Task<double> GetWalletBalanceFromTransactionsAsync()
        {
            var walletTransactions = await GetAllTransactionsAsync();
            if (!walletTransactions.Any())
                return 0d;

            var totalAmount = walletTransactions.Sum(wt => wt.TransactionAmount);

            return totalAmount;
        }

        public async Task<double> GetWalletBalaneFromLastTransaction()
        {
            var lastTransaction = await _walletTransactionRepository.Table.OrderBy(lt => lt.Id).LastAsync();
            if (lastTransaction == null)
                return 0d;

            return lastTransaction.WalletFinalAmount;
        }
    }
}
