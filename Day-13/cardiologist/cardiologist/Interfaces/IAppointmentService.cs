using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cardiologist.Models;

namespace cardiologist.Interfaces
{
    public interface IAppointmentService
    {
        int AddAppointment(Appointment appointment);
        List<Appointment>? SearchAppointments(AppointmentSearchModel searchModel);
    }
}
