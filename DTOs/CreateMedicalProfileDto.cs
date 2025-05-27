namespace ClinicAPI.DTOs
{
    public class CreateMedicalProfileDto
    {
        public string? BloodType { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicDiseases { get; set; }

        public int PatientId { get; set; }  // Required to link it to the patient
    }
}
