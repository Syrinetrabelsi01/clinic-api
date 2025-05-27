using ClinicAPI.Data;
using ClinicAPI.DTOs;
using ClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [Tags("6. Appointments")]
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            var doctor = await _context.Users.FindAsync(dto.DoctorId);
            var patient = await _context.Patients.FindAsync(dto.PatientId);

            if (doctor == null)
                return NotFound(new { message = "Doctor not found" });

            if (patient == null)
                return NotFound(new { message = "Patient not found" });

            var appointment = new Appointment
            {
                AppointmentDate = dto.AppointmentDate,
                Reason = dto.Reason,
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment created successfully" });
        }

        [HttpGet]
        public IActionResult GetAppointments(
            [FromQuery] string? doctorId,
            [FromQuery] int? patientId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var query = _context.Appointments
                .Where(a => true) // base query
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(doctorId))
                query = query.Where(a => a.DoctorId == doctorId);

            if (patientId.HasValue)
                query = query.Where(a => a.PatientId == patientId.Value);

            if (from.HasValue)
                query = query.Where(a => a.AppointmentDate >= from.Value);

            if (to.HasValue)
                query = query.Where(a => a.AppointmentDate <= to.Value);

            var appointments = query
                .Select(a => new
                {
                    a.Id,
                    a.AppointmentDate,
                    a.Reason,
                    DoctorName = a.Doctor.UserName,
                    PatientName = a.Patient.FullName
                })
                .ToList();

            return Ok(appointments);
        }

    }
}
