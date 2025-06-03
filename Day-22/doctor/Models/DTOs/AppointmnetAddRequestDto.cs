namespace doctor.Models.DTOs.AppointmnetAddRequestDto
{
    public class AppointmnetAddRequestDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmnetDateTime { get; set; }
        //public string? Description { get; set; }
        //public string? Status { get; set; } = "Pending";
    }
}