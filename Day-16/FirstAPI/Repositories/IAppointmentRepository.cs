using System.Collections.Generic;

public interface IAppointmentRepository
{
    List<Appointment> GetAll();
    Appointment? GetById(int id);
    void Add(Appointment appointment);
    void Update(Appointment appointment);
    void Delete(int id);
}
