namespace BankingAPI.Models
{
    public class TransactionLog
    {
        public int TransactionLogId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Type { get; set; } = string.Empty; // Deposit or Withdrawal
        public decimal Amount { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; } =  null!;
    }

}