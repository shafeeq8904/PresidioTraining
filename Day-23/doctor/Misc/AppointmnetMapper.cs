using doctor.Models.DTOs;

namespace doctor.Models.DTOs
{
    public class AppointmnetMapper
    {
        public AppointmnetResponseDto MapToResponseDto(Appointmnet result)
        {
            return new AppointmnetResponseDto
            {
                AppointmnetNumber = result.AppointmnetNumber,
                PatientId = result.PatientId,
                DoctorId = result.DoctorId,
                AppointmnetDateTime = result.AppointmnetDateTime,
                Status = result.Status,
                Doctor = result.Doctor == null ? null : new DoctorDto
                {
                    Id = result.Doctor.Id,
                    Name = result.Doctor.Name,
                    YearsOfExperience = result.Doctor.YearsOfExperience,
                    Email = result.Doctor.Email
                },
                Patient = result.Patient == null ? null : new PatientDto
                {
                    Id = result.Patient.Id,
                    Name = result.Patient.Name,
                    Age = result.Patient.Age,
                    Email = result.Patient.Email,
                    Phone = result.Patient.Phone
                }
            };
        }
    }
}