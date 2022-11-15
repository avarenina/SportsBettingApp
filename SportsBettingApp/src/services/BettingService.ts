import http from "@/http-common";
import BettingTicket from "@/types/BettingTicket";

/* eslint-disable */
class BettingService {


  placeBet(bettingTicket: BettingTicket): void {
    
    fetch("/betting/place-bet", {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(bettingTicket)
  })
    .then(response => response.json())
    .then(() => {
     
    })
    .catch(error => console.error('Unable to add item.', error));
}
    
   
  
}

export default new BettingService();
