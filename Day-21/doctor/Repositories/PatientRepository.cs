using doctor.Contexts;
using doctor.Interfaces;
using doctor.Models;
using Microsoft.EntityFrameworkCore;

namespace doctor.Repositories
{
    public  class PatientRepository : Repository<int, Patient>
    {
        public PatientRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<Patient> Get(int key)
        {
            var patient = await _clinicContext.patients.SingleOrDefaultAsync(p => p.Id == key);

            return patient??throw new Exception("No patient with the given ID");
        }

        public override async Task<IEnumerable<Patient>> GetAll()
        {
            return await _clinicContext.patients
            .Include(p => p.User)
            .ToListAsync();
        }
    }
}