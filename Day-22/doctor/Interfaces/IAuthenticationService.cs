
using doctor.Models.DTOs.DoctorSpecialities;

namespace doctor.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<UserLoginResponse> Login(UserLoginRequest user);
    }
}