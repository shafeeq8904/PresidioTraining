using doctor.Models;
using doctor.Models.DTOs.AppointmnetAddRequestDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace doctor.Interfaces
{
    public interface IAppointmnetService
    {
        Task<Appointmnet> AddAppointmnet(AppointmnetAddRequestDto appointmnetDto);
        Task<IEnumerable<Appointmnet>> GetAppointmnetsByPatientId(int patientId);
        Task<IEnumerable<Appointmnet>> GetAppointmnetsByDoctorId(int doctorId);
        Task<bool> CancelAppointment(string appointmentNumber);
    }
}