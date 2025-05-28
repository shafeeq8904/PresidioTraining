using doctor.Contexts;
using doctor.Interfaces;
using doctor.Models;
using Microsoft.EntityFrameworkCore;

namespace doctor.Repositories
{
    public  class Patinet : Repository<int, Patient>
    {
        protected Patinet(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<Patient> Get(int key)
        {
            var patient = await _clinicContext.patients.SingleOrDefaultAsync(p => p.Id == key);

            return patient??throw new Exception("No patient with teh given ID");
        }

        public override async Task<IEnumerable<Patient>> GetAll()
        {
            var patients = _clinicContext.patients;
            if (patients.Count() == 0)
                throw new Exception("No Patients in the database");
            return await patients.ToListAsync();
        }
    }
}