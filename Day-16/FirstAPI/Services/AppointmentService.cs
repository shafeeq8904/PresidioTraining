public class AppointmentService
{
    private readonly IAppointmentRepository _repository;

    public AppointmentService(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public List<Appointment> GetAllAppointments()
    {
        return _repository.GetAll();
    }

    public Appointment? GetAppointmentById(int id)
    {
        return _repository.GetById(id);
    }

    public void AddAppointment(Appointment appointment)
    {
        _repository.Add(appointment);
    }

    public void UpdateAppointment(Appointment appointment)
    {
        _repository.Update(appointment);
    }

    public void DeleteAppointment(int id)
    {
        _repository.Delete(id);
    }
}
