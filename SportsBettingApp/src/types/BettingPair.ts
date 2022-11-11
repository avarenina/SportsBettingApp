import Tip from "@/types/Tip"


export default interface BettingPair {
    id: number;
    firstOpponent: string;
    secondOpponent: string;
    matchStartUTC: string;
    sportId: number;
    tips: Tip[];
}