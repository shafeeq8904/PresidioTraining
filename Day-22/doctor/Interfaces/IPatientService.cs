using doctor.Models;
using doctor.Models.DTOs.DoctorSpecialities;
using doctor.Models.DTOs.Patients;

namespace doctor.Interfaces
{
    public interface IPatientService
    {
        public Task<Patient> GetPatientByName(string name);
        public Task<Patient> AddPatient(PatientAddRequestDto patient);
    }
}