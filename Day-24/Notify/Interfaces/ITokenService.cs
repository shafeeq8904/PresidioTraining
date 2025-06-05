using Notify.Models;

namespace Notify.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
