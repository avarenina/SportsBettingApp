import Tip from "@/types/Tip";
import BettingPair from "./BettingPair";

export default interface SpecialOffer {
    id?: number;
    bettingPair: BettingPair;
    sportId: number;
    tips: Tip[];
}