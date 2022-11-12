namespace SportsBettingApp_Backend.Models
{
    public class BettingPair
    {
        public int ID { get; set; }
        public string FirstOpponent { get; set; }
        public string SecondOpponent { get; set; }
        public DateTime MatchStartUTC { get; set; }
        public Sport Sport { get; set; }
        public int CategoryId { get; set; }
        public List<Tip> Tips { get; set; }
    }
}



