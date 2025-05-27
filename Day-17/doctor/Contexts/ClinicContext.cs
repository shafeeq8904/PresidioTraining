using doctor.Models;
using Microsoft.EntityFrameworkCore;

namespace doctor.Contexts
{
    public class ClinicContext : DbContext
    {
      
        public ClinicContext(DbContextOptions options) :base(options)
        {
            
        }
        public DbSet<Patient> patients { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Appointmnet> appointmnets { get; set; }
        public DbSet<Speciality> specialities { get; set; }
        public DbSet<DoctorSpeciality> doctorspecialities { get; set; }

    }
}