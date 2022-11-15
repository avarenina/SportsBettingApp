global using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class BettingTicket : BaseEntity
    {
       
        [JsonPropertyName("ticketPairs")]
        public List<TicketPair> TicketPairs { get; set; }

        [JsonPropertyName("betAmount")]
        public double BetAmount { get; set; }

        [JsonPropertyName("ticketPlacedTime")]
        public DateTime TicketPlacedTime { get; set; }

        [JsonPropertyName("isWinningTicket")]
        public bool IsWinningTicket { get; set; }
    }
}
