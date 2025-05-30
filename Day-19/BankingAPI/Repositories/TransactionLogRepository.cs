using BankingAPI.Data;
using BankingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Repositories
{
    public class TransactionLogRepository : Repository<int, TransactionLog>
    {
        public TransactionLogRepository(BankingContext context) : base(context)
        {
        }

        public override async Task<TransactionLog> Get(int key)
        {
            var transaction = await _context.TransactionLogs
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.TransactionLogId == key); 

            return transaction ?? throw new Exception("Transaction not found with the given ID");
        }

        public override async Task<IEnumerable<TransactionLog>> GetAll()
        {
            var transactions = await _context.TransactionLogs
                .Include(t => t.Account)
                .ToListAsync();

            if (!transactions.Any())
                throw new Exception("No transactions found in the database");

            return transactions;
        }

        public async Task<IEnumerable<TransactionLog>> GetByAccountId(int accountId)
        {
            return await _context.TransactionLogs
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
        }
    }
}
