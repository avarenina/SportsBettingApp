global using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class BettingPair : BaseEntity
    {
       
        [JsonPropertyName("firstOpponent")]
        public string FirstOpponent { get; set; }

        [JsonPropertyName("secondOpponent")]
        public string SecondOpponent { get; set; }

        [JsonPropertyName("matchStartUTC")]
        public DateTime MatchStartUTC { get; set; }

        [JsonPropertyName("sport")]
        public Sport Sport { get; set; }

        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("tips")]
        public List<Tip> Tips { get; set; }
    }
}



