namespace BankingAPI.DTOs
{
    public class TransactionLogResponseDto
    {
        public int TransactionLogId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }


}