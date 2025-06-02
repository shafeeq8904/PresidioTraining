using doctor.Models;
using doctor.Models.DTOs.DoctorSpecialities;

namespace doctor.Interfaces
{
    public interface IOtherContextFunctionities
    {
        public Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string specilaity);
    }
}