import Tip from "@/types/Tip"
import BettingPair from "@/types/BettingPair"

export default interface SelectedPair {
    bettingPair: BettingPair;
    tip: Tip;
    isSpecialOffer?: boolean;
}