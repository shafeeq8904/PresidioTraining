using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using doctor.Interfaces;
using doctor.Models;
using System.Security.Claims;

namespace doctor.Policies
{
    public class DoctorExperienceHandler : AuthorizationHandler<DoctorExperienceRequirement>
    {
        private readonly IRepository<int, Doctor> _doctorRepository;

        public DoctorExperienceHandler(IRepository<int, Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DoctorExperienceRequirement requirement)
        {
            // The resource is the appointment
            var appointment = context.Resource as Appointmnet;
            if (appointment == null)
                return;

            var emailClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)
                        ?? context.User.FindFirst(ClaimTypes.Email)
                        ?? context.User.FindFirst("email");

            if (emailClaim == null)
                return;

            string doctorEmail = emailClaim.Value;

            var allDoctors = await _doctorRepository.GetAll();
            var doctor = allDoctors.FirstOrDefault(d => d.Email.Equals(doctorEmail, System.StringComparison.OrdinalIgnoreCase));

            if (doctor == null)
                return;

            
            if (doctor.YearsOfExperience >= requirement.MinimumYears && doctor.Id == appointment.DoctorId)
            {
                context.Succeed(requirement);
            }
        }
    }
}