global using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class SpecialOffer: BaseEntity
    {
        public BettingPair BettingPair { get; set; }
        public int SportId { get; set; }
        public List<Tip> Tips { get; set; }
    }
}
