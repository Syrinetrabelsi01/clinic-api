using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.DTOs
{
    public class CreatePatientDto
    {
        [Required]
        public string? FullName { get; set; }

        [Required]
        [RegularExpression("Male|Female", ErrorMessage = "Gender must be either 'Male' or 'Female'")]
        public string? Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Phone]
        public string? Phone { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
