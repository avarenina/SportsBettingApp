global using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Tip : BaseEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("stake")]
        public double Stake { get; set; }
    }
}
