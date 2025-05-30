using BankingAPI.DTOs;
using BankingAPI.Interfaces;
using BankingAPI.Models;
using BankingAPI.Repositories;

namespace BankingAPI.Services
{
    public class TransactionLogService : ITransactionLogService
    {
        private readonly TransactionLogRepository _transactionRepo;
        private readonly AccountRepository _accountRepo;

        public TransactionLogService(TransactionLogRepository transactionRepo, AccountRepository accountRepo)
        {
            _transactionRepo = transactionRepo;
            _accountRepo = accountRepo;
        }

        public async Task<TransactionLogResponseDto> Deposit(TransactionLogRequestDto dto)
        {
            var account = await _accountRepo.GetByAccountNumber(dto.AccountNumber);

            if (account == null)
                throw new Exception("Account not found");

            account.Balance += dto.Amount;

            var transaction = new TransactionLog
            {
                AccountId = account.AccountId,
                Amount = dto.Amount,
                Type = "deposit",
                Timestamp = DateTime.UtcNow
            };

            await _transactionRepo.Add(transaction);
            await _accountRepo.Update(account.AccountId, account);

            return MapToResponseDto(transaction);
        }

        public async Task<TransactionLogResponseDto> Withdraw(TransactionLogRequestDto dto)
        {
            var account = await _accountRepo.GetByAccountNumber(dto.AccountNumber);

            if (account == null)
                throw new Exception("Account not found");

            if (account.Balance < dto.Amount)
                throw new Exception("Insufficient balance");

            account.Balance -= dto.Amount;

            var transaction = new TransactionLog
            {
                AccountId = account.AccountId,
                Amount = dto.Amount,
                Type = "withdraw",
                Timestamp = DateTime.UtcNow
            };

            await _transactionRepo.Add(transaction);
            await _accountRepo.Update(account.AccountId, account);

            return MapToResponseDto(transaction);
        }


        public async Task<IEnumerable<TransactionLogResponseDto>> GetAllTransactions()
        {
            var transactions = await _transactionRepo.GetAll();
            return transactions.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<TransactionLogResponseDto>> GetTransactionsForAccount(int accountId)
        {
            var transactions = await _transactionRepo.GetByAccountId(accountId);
            return transactions.Select(MapToResponseDto);
        }

        private TransactionLogResponseDto MapToResponseDto(TransactionLog t) => new TransactionLogResponseDto
        {
            TransactionLogId = t.TransactionLogId,
            AccountId = t.AccountId,
            Amount = t.Amount,
            Type = t.Type,
            Timestamp = t.Timestamp
        };
    }
}
