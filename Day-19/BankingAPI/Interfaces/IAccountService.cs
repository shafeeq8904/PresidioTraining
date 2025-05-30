using BankingAPI.DTOs;

namespace BankingAPI.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResponseDto> AddAccount(AccountRequestDto accountDto);
        Task<AccountResponseDto> GetAccountById(int id);
        Task<IEnumerable<AccountResponseDto>> GetAllAccounts();
        
    }
}
