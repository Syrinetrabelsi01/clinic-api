namespace ClinicAPI.DTOs
{
    public class CreateAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; }

        public string DoctorId { get; set; }
        public int PatientId { get; set; }
    }
}
