using doctor.Contexts;
using doctor.Models;
using Microsoft.EntityFrameworkCore;

namespace doctor.Repositories
{
    public  class DoctorRepository : Repository<int, Doctor>
    {
        protected DoctorRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<Doctor> Get(int key)
        {
            var doctor = await _clinicContext.doctors.SingleOrDefaultAsync(p => p.Id == key);

            return doctor??throw new Exception("No doctor with teh given ID");
        }

        public override async Task<IEnumerable<Doctor>> GetAll()
        {
            var doctors = _clinicContext.doctors;
            if (doctors.Count() == 0)
                throw new Exception("No Doctor in the database");
            return await doctors.ToListAsync();
        }
    }
}
