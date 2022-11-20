
enum TransactionType {
  Deposit,
  Withdraw,
  Bet,
  Win
}

interface WalletTransaction
{
    id?: number;
    transactionTimestamp: string;
    transactionAmount: number;
    transactionType: TransactionType;
    walletFinalAmount: number;
}

export { WalletTransaction, TransactionType };