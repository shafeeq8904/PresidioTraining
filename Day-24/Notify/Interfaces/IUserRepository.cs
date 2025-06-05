using Notify.Models;
using System.Threading.Tasks;

namespace Notify.Interfaces
{
    public interface IUserRepository : IRepository<int, User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}
