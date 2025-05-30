using BankingAPI.Data;
using BankingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Repositories
{
    public class CustomerRepository : Repository<int, Customer>
    {
        public CustomerRepository(BankingContext context) : base(context) {}

        public override async Task<Customer> Get(int key)
        {
            return await _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(a => a.Transactions)
                .FirstOrDefaultAsync(c => c.CustomerId == key)
                ?? throw new Exception("Customer not found");
        }

        public override async Task<IEnumerable<Customer>> GetAll()
        {
            var list = await _context.Customers.Include(c => c.Accounts).ToListAsync();
            if (!list.Any())
                throw new Exception("No customers found");
            return list;
        }
    }
}
