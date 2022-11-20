global using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public enum TransactionType
    {
        Deposit,
        Withdraw,
        Bet,
        Win
    }

    public class WalletTransaction : BaseEntity
    {
        public DateTime TransactionTimestamp { get; set; }

        public double TransactionAmount { get; set; }

        public TransactionType TransactionType { get; set; }

        public double WalletFinalAmount { get; set; }

    }
}
