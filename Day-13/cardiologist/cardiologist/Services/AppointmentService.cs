using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cardiologist.Interfaces;
using cardiologist.Models;

namespace cardiologist.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositor<int, Appointment> _appointmentRepository;

        public AppointmentService(IRepositor<int, Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public int AddAppointment(Appointment appointment)
        {
            try
            {
                var result = _appointmentRepository.Add(appointment);
                return result.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }

        public List<Appointment>? SearchAppointments(AppointmentSearchModel searchModel)
        {
            try
            {
                var appointments = _appointmentRepository.GetAll();
                appointments = FilterByName(appointments, searchModel.PatientName);
                appointments = FilterByDate(appointments, searchModel.AppointmentDate);
                appointments = FilterByAge(appointments, searchModel.AgeRange);

                return appointments.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            } 
        }

        private ICollection<Appointment> FilterByName(ICollection<Appointment> list, string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return list;
            return list.Where(a => a.PatientName.ToLower().Contains(name.ToLower())).ToList();
        }

        private ICollection<Appointment> FilterByDate(ICollection<Appointment> list, DateTime? date)
        {
            if (date == null) return list;
            return list.Where(a => a.AppointmentDate.Date == date.Value.Date).ToList();
        }

        private ICollection<Appointment> FilterByAge(ICollection<Appointment> list, Range<int>? range)
        {
            if (range == null || range.MinVal == null || range.MaxVal == null) return list;
            return list.Where(a => a.PatientAge >= range.MinVal && a.PatientAge <= range.MaxVal).ToList();
        }
    }
}
