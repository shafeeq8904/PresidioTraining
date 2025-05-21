namespace cardiologist.Models
{
    public class Appointment : IComparable<Appointment>, IEquatable<Appointment>
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;

        

        public override string ToString()
        {
            return $"ID: {Id}\nName: {PatientName}\nAge: {PatientAge}\nDate: {AppointmentDate}\nReason: {Reason}";
        }

        public int CompareTo(Appointment? other)
        {
            return this.Id.CompareTo(other?.Id);
        }

        public bool Equals(Appointment? other)
        {
            return this.Id == other?.Id;
        }
    }
}
