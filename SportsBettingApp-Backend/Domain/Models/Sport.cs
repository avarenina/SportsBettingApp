global using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Sport : BaseEntity
    {
       
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("availableTips")]
        public List<Tip> AvailableTips { get; set; }
    }
}
