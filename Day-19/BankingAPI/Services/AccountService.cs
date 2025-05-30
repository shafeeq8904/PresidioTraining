using BankingAPI.DTOs;
using BankingAPI.Interfaces;
using BankingAPI.Models;
using BankingAPI.Repositories;

namespace BankingAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<int, Account> _accountRepository;

        public AccountService(IRepository<int, Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountResponseDto> AddAccount(AccountRequestDto accountDto)
        {
            var account = new Account
            {
                AccountNumber = Guid.NewGuid().ToString("N")[..10],
                Balance = accountDto.Balance,
                CustomerId = accountDto.CustomerId
            };

            var result = await _accountRepository.Add(account);
            return ToResponseDto(result);
        }

        public async Task<IEnumerable<AccountResponseDto>> GetAllAccounts()
        {
            var accounts = await _accountRepository.GetAll();
            return accounts.Select(a => ToResponseDto(a)).ToList();
        }

        public async Task<AccountResponseDto> GetAccountById(int id)
        {
            var account = await _accountRepository.Get(id);
            return ToResponseDto(account);
        }

        
        private AccountResponseDto ToResponseDto(Account account)
        {
            return new AccountResponseDto
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                CustomerId = account.CustomerId
            };
        }
    }
}
