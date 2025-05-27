using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace doctor.Models
{
    public class DoctorSpeciality
    {
        [Key]
        public int SerialNumber { get; set; }
        public int DoctorId { get; set; }
        public int SpecialityId { get; set; }

        [ForeignKey("DoctorId")]
        public Speciality? Speciality { get; set; }

        [ForeignKey("SpecialityId")]
        public Doctor? Doctor { get; set; }
    }
}