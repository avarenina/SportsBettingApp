import BettingTicket from "@/types/BettingTicket";
import http from "@/http-common";

/* eslint-disable */
class BettingService {
  placeBet(bettingTicket: BettingTicket): Promise<any> {
    return http.post("/api/betting/place-bet", bettingTicket);
  }

  getBettingTickets(): Promise<any> {
    return http.get("/api/betting/betting-tickets");
  }
}

export default new BettingService();
