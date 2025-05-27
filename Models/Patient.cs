using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // ✅ Fixes CS1061 errors:
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public MedicalProfile? MedicalProfile { get; set; }
        


    }
}
