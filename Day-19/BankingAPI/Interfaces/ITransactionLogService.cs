using BankingAPI.DTOs;

namespace BankingAPI.Interfaces
{
    public interface ITransactionLogService
    {
        Task<TransactionLogResponseDto> Deposit(TransactionLogRequestDto transactionDto);
        Task<TransactionLogResponseDto> Withdraw(TransactionLogRequestDto transactionDto);
        Task<IEnumerable<TransactionLogResponseDto>> GetAllTransactions();
        Task<IEnumerable<TransactionLogResponseDto>> GetTransactionsForAccount(int accountId);

    }
}
