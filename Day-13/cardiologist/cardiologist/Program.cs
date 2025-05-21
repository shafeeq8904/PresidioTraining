using cardiologist.Interfaces;
using cardiologist.Models;
using cardiologist.Repositories;
using cardiologist.Services;
using cardiologist;

namespace Cardiologist
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IRepositor<int, Appointment> appointmentRepo = new AppointmentRepository();
            IAppointmentService appointmentService = new AppointmentService(appointmentRepo);
            ManageAppointment manageApp = new ManageAppointment(appointmentService);
            manageApp.Start();
        }
    }
}