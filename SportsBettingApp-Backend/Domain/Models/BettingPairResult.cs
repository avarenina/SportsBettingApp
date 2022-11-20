
namespace Domain.Models
{
    public class BettingPairResult : BaseEntity
    {
        public BettingPair BettingPair { get; set; }
        public int FirstOpponentScore { get; set; }
        public int SecondOpponentScore { get; set; }
        public string WinningTip { get; set; }
    }
}
