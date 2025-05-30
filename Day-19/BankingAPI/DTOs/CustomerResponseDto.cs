namespace BankingAPI.DTOs
{
   public class CustomerResponseDto
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public List<AccountResponseDto> Accounts { get; set; } = new();
}

}