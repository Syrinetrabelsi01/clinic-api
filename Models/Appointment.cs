using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string? Notes { get; set; }

        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        public string? AppUserId { get; set; }  // Doctor
        public AppUser? AppUser { get; set; }
    }
}
