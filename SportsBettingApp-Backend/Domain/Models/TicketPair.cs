global using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class TicketPair : BaseEntity
    {
       
        [JsonPropertyName("bettingPair")]
        public BettingPair BettingPair { get; set; }

        [JsonPropertyName("tip")]
        public Tip Tip { get; set; }
        
        // Add score and other stuff
    }
}
