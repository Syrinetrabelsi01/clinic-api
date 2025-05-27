using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicAPI.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public string? Diagnosis { get; set; }
        public string? Medications { get; set; }
        public DateTime CreatedAt { get; set; }

        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        public string? FilePath { get; set; }  // Optional file upload
    }
}
