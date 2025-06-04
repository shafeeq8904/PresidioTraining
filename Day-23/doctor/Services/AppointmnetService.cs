using doctor.Interfaces;
using doctor.Models;
using doctor.Models.DTOs.AppointmnetAddRequestDto;
using doctor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doctor.Services
{
    public class AppointmnetService : IAppointmnetService
    {
        private readonly IRepository<string, Appointmnet> _appointmnetRepository;
        private readonly IRepository<int, Patient> _patientRepository;
        private readonly IRepository<int, Doctor> _doctorRepository;

        public AppointmnetService(IRepository<string, Appointmnet> appointmnetRepository,
                                IRepository<int, Doctor> doctorRepository,
                                IRepository<int, Patient> patientRepository)
        {
            _appointmnetRepository = appointmnetRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task<Appointmnet> AddAppointmnet(AppointmnetAddRequestDto appointmnetDto)
        {
            var doctor = await _doctorRepository.Get(appointmnetDto.DoctorId);
            if (doctor == null)
                throw new Exception("Doctor not found");

            var patient = await _patientRepository.Get(appointmnetDto.PatientId);
            if (patient == null)
                throw new Exception("Patient not found");

            var appointmnet = new Appointmnet
            {
                AppointmnetNumber = $"A{DateTime.UtcNow.Ticks}", // or Guid.NewGuid().ToString()
                PatientId = appointmnetDto.PatientId,
                DoctorId = appointmnetDto.DoctorId,
                AppointmnetDateTime = appointmnetDto.AppointmnetDateTime,
                Status = "Booked"
            };

            var result = await _appointmnetRepository.Add(appointmnet);
            if (result == null)
                throw new Exception("Could not add appointment");

            return result;
        }

        public async Task<IEnumerable<Appointmnet>> GetAppointmnetsByPatientId(int patientId)
        {
            var all = await _appointmnetRepository.GetAll();
            return all.Where(a => a.PatientId == patientId);
        }

        public async Task<IEnumerable<Appointmnet>> GetAppointmnetsByDoctorId(int doctorId)
        {
            var all = await _appointmnetRepository.GetAll();
            return all.Where(a => a.DoctorId == doctorId);
        }

        public async Task<bool> CancelAppointment(string appointmentNumber)
        {
            var appointment = await _appointmnetRepository.Get(appointmentNumber);
            if (appointment == null)
                return false;

            appointment.Status = "Cancelled";
            await _appointmnetRepository.Update(appointmentNumber, appointment);
            return true;
        }
    }

}