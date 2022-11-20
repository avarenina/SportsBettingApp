import http from "@/http-common";
import { WalletTransaction } from "@/types/WalletTransaction";

/* eslint-disable */
class WalletService {
  getAllTransactions(): Promise<any> {
    return http.get("/api/wallet/transactions");
  }
  getWalletBalance(): Promise<any> {
    return http.get("/api/wallet/wallet-balance");
  }
  deposit(walletTransaction: WalletTransaction): Promise<any> {
    return http.post("/api/wallet/deposit", walletTransaction);
  }
}

export default new WalletService();