import SelectedPair from "@/types/SelectedPair";

export default interface BettingTicket {
    id?: number
    ticketPairs: SelectedPair [];
    betAmount: number;
    ticketPlacedTime?: string;
    isWinningTicket?: boolean;
}