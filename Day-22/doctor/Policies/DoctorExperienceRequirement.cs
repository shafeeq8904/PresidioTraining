using Microsoft.AspNetCore.Authorization;

namespace doctor.Policies
{
    public class DoctorExperienceRequirement : IAuthorizationRequirement
    {
        public int MinimumYears { get; }
        public DoctorExperienceRequirement(int minimumYears)
        {
            MinimumYears = minimumYears;
        }
    }
}