using Microsoft.EntityFrameworkCore;
using Notify.Data;
using Notify.Interfaces;
using Notify.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notify.Repositories
{
    public class UserRepository : Repository<int, User>, IUserRepository
    {
        public UserRepository(NotifyContext context) : base(context) { }

        public override async Task<User> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
