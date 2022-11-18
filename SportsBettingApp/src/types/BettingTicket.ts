import SelectedPair from "@/types/SelectedPair";

export default interface BettingTicket {
    id?: number
    ticketPairs: SelectedPair [];
    betAmount: number;
    betAmountFinal: number;
    ticketPlacedTime?: string;
    isWinningTicket?: boolean;
    isCompleted?:boolean;
    winAmount: number;
    totalStake:number;
    taxAmount:number;
    payoutAmount: number;
    manipulationCost: number;
}