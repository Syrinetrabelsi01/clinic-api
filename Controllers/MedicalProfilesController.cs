using ClinicAPI.Data;
using ClinicAPI.DTOs;
using ClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [Tags("5. Medical Profiles")]
    [ApiController]
    [Route("api/medical-profiles")]
    public class MedicalProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicalProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMedicalProfileDto dto)
        {
            var patient = await _context.Patients.FindAsync(dto.PatientId);
            if (patient == null)
                return NotFound(new { message = "Patient not found" });

            if (patient.MedicalProfile != null)
                return BadRequest(new { message = "Medical profile already exists for this patient" });

            var profile = new MedicalProfile
            {
                BloodType = dto.BloodType,
                Allergies = dto.Allergies,
                ChronicDiseases = dto.ChronicDiseases,
                PatientId = dto.PatientId
            };

            _context.MedicalProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Medical profile created successfully" });
        }
    }
}
