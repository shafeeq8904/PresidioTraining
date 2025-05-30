namespace BankingAPI.Models
{
    public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}

}