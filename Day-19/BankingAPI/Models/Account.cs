namespace BankingAPI.Models
{
    public class Account
{
    public int AccountId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public ICollection<TransactionLog> Transactions { get; set; } = new List<TransactionLog>();
}

}
