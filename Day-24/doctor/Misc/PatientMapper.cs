using doctor.Models;
using doctor.Models.DTOs.DoctorSpecialities;
using  doctor.Models.DTOs.Patients;

namespace doctor.Misc
{
    public class PatientMapper
    {
    public Patient? MapPatientAddRequestPatient(PatientAddRequestDto addRequestDto)
        {
            Patient patient = new Patient();
            patient.Name = addRequestDto.Name;
            patient.Age = addRequestDto.Age;
            patient.Phone = addRequestDto.Phone;
            patient.Email = addRequestDto.Email;
            return patient;
        }        
    }
}