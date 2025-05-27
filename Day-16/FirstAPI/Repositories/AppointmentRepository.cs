using System.Collections.Generic;
using System.Linq;

public class AppointmentRepository : IAppointmentRepository
{
    private static List<Appointment> appointments = new List<Appointment>();

    public List<Appointment> GetAll() => {
        appointments;
    }

    public Appointment? GetById(int id) =>{
        appointments.FirstOrDefault(a => a.Id == id);
    }

    public void Add(Appointment appointment)
    {
        if (appointments.Any(a => a.Id == appointment.Id))
        {
            throw new System.Exception($"Appointment with ID {appointment.Id} already exists.");
        }
        appointments.Add(appointment);
    }

    public void Update(Appointment appointment)
    {
        var existing = GetById(appointment.Id);
        if (existing != null)
        {
            existing.PatientId = appointment.PatientId;
            existing.AppointmentDate = appointment.AppointmentDate;
            existing.Description = appointment.Description;
        }
    }

    public void Delete(int id)
    {
        var appointment = GetById(id);
        if (appointment != null)
        {
            appointments.Remove(appointment);
        }
    }
}
