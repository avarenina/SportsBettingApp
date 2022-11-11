namespace SportsBettingApp_Backend.Models
{
    public class BettingDay
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public string Label { get; set; }

        public string QueryStringId { get; set; }

    }
}