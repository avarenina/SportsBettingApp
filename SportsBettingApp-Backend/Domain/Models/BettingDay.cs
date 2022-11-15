global using Domain.Common;
namespace Domain.Models
{
    public class BettingDay : BaseEntity
    {
        public DateTime Date { get; set; }

        public string Label { get; set; }

        public string QueryStringId { get; set; }

    }
}