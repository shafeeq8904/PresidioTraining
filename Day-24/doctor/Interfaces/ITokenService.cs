using doctor.Models;

namespace doctor.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}