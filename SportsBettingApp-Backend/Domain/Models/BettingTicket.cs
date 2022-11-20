global using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class BettingTicket : BaseEntity
    {
       
        [JsonPropertyName("ticketPairs")]
        public List<TicketPair> TicketPairs { get; set; } = new List<TicketPair>();

        [JsonPropertyName("betAmount")]
        public double BetAmount { get; set; }

        public double BetAmountFinal { get; set; }

        [JsonPropertyName("ticketPlacedTime")]
        public DateTime TicketPlacedTime { get; set; }

        [JsonPropertyName("isWinningTicket")]
        public bool IsWinningTicket { get; set; }

        public bool IsCompleted { get; set; }

        public double WinAmount { get; set; }

        public double TotalStake { get; set; }

        public double TaxAmount { get; set; }

        public double PayoutAmount { get; set; }

        public double ManipulationCost { get; set; }

        public WalletTransaction? WalletTransaction { get; set; }
    }
}
