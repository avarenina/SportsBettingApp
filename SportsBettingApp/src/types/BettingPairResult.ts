import BettingPair from "@/types/BettingPair"

export default interface BettingPairResult {
    bettingPair: BettingPair;
    firstOpponentScore: number;
    secondOpponentScore: number;
    winningTip: string;
}