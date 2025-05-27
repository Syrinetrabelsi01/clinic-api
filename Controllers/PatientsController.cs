using AutoMapper;
using ClinicAPI.Data;
using ClinicAPI.DTOs;
using ClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.Controllers
{
    [Tags("4. Patients")]
    [ApiController]
    [Route("api/patients")]
    //[AllowAnonymous] 
    [Authorize(Roles = "Doctor,Receptionist")]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PatientsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            Console.WriteLine("🔥 New PatientsController loaded"); // Debug
        }

        // GET: api/patients
        [HttpGet]

        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<PatientDto>>(patients));
        }

        // GET: api/patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound(new { message = "Patient not found" });

            return Ok(_mapper.Map<PatientDto>(patient));
        }

        // POST: api/patients
        [HttpPost]
        public async Task<ActionResult<PatientDto>> CreatePatient([FromBody] CreatePatientDto dto)
        {
            Console.WriteLine("📦 Received POST /api/patients");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                Console.WriteLine("❌ Model validation failed");
                foreach (var err in errors)
                    Console.WriteLine($"- {err}");

                return BadRequest(new
                {
                    message = "Invalid input",
                    errors = errors
                });
            }

            try
            {
                var patient = _mapper.Map<Patient>(dto);
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<PatientDto>(patient);
                Console.WriteLine("✅ Patient created with ID: " + result.Id);

                return Ok(new
                {
                    message = "Patient added successfully",
                    patientId = result.Id
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 Exception while creating patient: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }



        // PUT: api/patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientDto dto)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound(new { message = "Patient not found" });

            // 🔁 Only update fields that are present
            if (!string.IsNullOrWhiteSpace(dto.FullName)) patient.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Gender)) patient.Gender = dto.Gender;
            if (dto.DateOfBirth.HasValue) patient.DateOfBirth = dto.DateOfBirth.Value;
            if (!string.IsNullOrWhiteSpace(dto.Phone)) patient.Phone = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.Email)) patient.Email = dto.Email;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Patient updated successfully" });
        }


        // DELETE: api/patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound(new { message = "Patient not found" });

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Patient deleted successfully" });
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPatient(int id, [FromBody] UpdatePatientDto dto)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound(new { message = "Patient not found" });

            if (!string.IsNullOrWhiteSpace(dto.FullName)) patient.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Gender)) patient.Gender = dto.Gender;
            if (dto.DateOfBirth.HasValue) patient.DateOfBirth = dto.DateOfBirth.Value;
            if (!string.IsNullOrWhiteSpace(dto.Phone)) patient.Phone = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.Email)) patient.Email = dto.Email;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Patient partially updated" });
        }


        [HttpGet("debug")]
        [AllowAnonymous]
        public async Task<IActionResult> DebugPing()
        {
            Console.WriteLine("✅ Debug endpoint hit in PatientsController");
            var total = await _context.Patients.CountAsync();
            return Ok(new { message = "PatientsController is active", totalPatients = total });
        }

        [HttpGet("short")]
        [AllowAnonymous] // or [Authorize] if you prefer
        public IActionResult GetPatientList()
        {
            var patients = _context.Patients
                .Select(p => new
                {
                    p.Id,
                    p.FullName
                })
                .ToList();

            return Ok(patients);
        }

    }
}
