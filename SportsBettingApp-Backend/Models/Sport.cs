namespace SportsBettingApp_Backend.Models
{
    public class Sport
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<Tip> AvailableTips { get; set; }
    }
}
