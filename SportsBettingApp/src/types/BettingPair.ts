import Tip from "@/types/Tip"
import Sport from "@/types/Sport"

export default interface BettingPair {
    id: number;
    firstOpponent: string;
    secondOpponent: string;
    matchStartUTC: string;
    sport: Sport;
    tips: Tip[];
}