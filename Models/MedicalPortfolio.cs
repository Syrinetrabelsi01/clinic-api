namespace ClinicAPI.Models
{
    public class MedicalProfile
    {
        public int Id { get; set; }

        public string? BloodType { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicDiseases { get; set; }

        // Foreign key to Patient
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
