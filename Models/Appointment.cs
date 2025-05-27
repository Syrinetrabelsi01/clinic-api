namespace ClinicAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }

        // 🔗 Doctor (AppUser)
        public string DoctorId { get; set; }
        public AppUser Doctor { get; set; }

        // 🔗 Patient
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
