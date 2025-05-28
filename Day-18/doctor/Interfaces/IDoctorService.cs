using doctor.Models;
using doctor.Models.DTOs.DoctorSpecialities;

namespace doctor.Interfaces
{
    public interface IDoctorService
    {
        public Task<Doctor> GetDoctByName(string name);
        public Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality);
        public Task<Doctor> AddDoctor(DoctorAddRequestDto doctor);

    }
}