using Microsoft.AspNetCore.Identity;

namespace ClinicAPI.Models
{
    public class AppUser : IdentityUser
    {
        // This field helps distinguish doctors/receptionists/admins
        public string? Role { get; set; }

        // Navigation property for doctor’s appointments
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
