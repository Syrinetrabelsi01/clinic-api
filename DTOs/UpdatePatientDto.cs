namespace ClinicAPI.DTOs
{
    public class UpdatePatientDto
    {
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
