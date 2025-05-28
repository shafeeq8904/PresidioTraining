using doctor.Contexts;
using doctor.Models;
using Microsoft.EntityFrameworkCore;

namespace doctor.Repositories
{
    public class AppointmnetRepository : Repository<string, Appointmnet>
    {
        public AppointmnetRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<Appointmnet> Get(string key)
        {
            var appointment = await _clinicContext.appointmnets
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .SingleOrDefaultAsync(a => a.AppointmnetNumber == key);

            return appointment ?? throw new Exception("No appointment with the given ID");
        }

        public override async Task<IEnumerable<Appointmnet>> GetAll()
        {
            var appointments = _clinicContext.appointmnets;
            if (!await appointments.AnyAsync())
                throw new Exception("No Appointments in the database");
            return await appointments.ToListAsync();
        }
    }
}
