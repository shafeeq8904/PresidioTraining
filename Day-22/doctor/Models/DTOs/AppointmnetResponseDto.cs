namespace doctor.Models.DTOs
{
    public class AppointmnetResponseDto
    {
        public string AppointmnetNumber { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmnetDateTime { get; set; }
        public string Status { get; set; }
        public DoctorDto Doctor { get; set; }
        public PatientDto Patient { get; set; }
    }

    public class DoctorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float YearsOfExperience { get; set; }
        public string Email { get; set; }
    }

    public class PatientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}