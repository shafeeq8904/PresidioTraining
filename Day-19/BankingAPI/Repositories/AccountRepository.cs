using BankingAPI.Data;
using BankingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Repositories
{
    public class AccountRepository : Repository<int, Account>
    {
        public AccountRepository(BankingContext context) : base(context)
        {
        }

        public async Task<Account?> GetByAccountNumber(string accountNumber)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }

        public override async Task<Account> Get(int key)
        {
            var account = await _context.Accounts
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.AccountId == key);

            return account ?? throw new Exception("Account not found with the given ID");
        }

        public override async Task<IEnumerable<Account>> GetAll()
        {
            var accounts = await _context.Accounts
                .Include(a => a.Customer)
                .ToListAsync();

            if (!accounts.Any())
                throw new Exception("No accounts found in the database");

            return accounts;
        }
    }
}
