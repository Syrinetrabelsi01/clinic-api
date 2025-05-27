using ClinicAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.Controllers
{
    [Tags("2. Dashboard")]
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalPatients = await _context.Patients.CountAsync();
            var genderDistribution = await _context.Patients
                .GroupBy(p => p.Gender.ToLower()) // normalize casing
                .Select(g => new
                {
                    gender = g.Key,
                    count = g.Count()
                })
                .ToDictionaryAsync(g => g.gender, g => g.count);

            var totalAppointments = await _context.Appointments.CountAsync();

            var today = DateTime.Today;
            var appointmentsToday = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == today)
                .CountAsync();

            var topDoctors = await _context.Appointments
                .GroupBy(a => a.Doctor.UserName)
                .Select(g => new
                {
                    name = g.Key,
                    appointmentCount = g.Count()
                })
                .OrderByDescending(g => g.appointmentCount)
                .Take(5)
                .ToListAsync();

            return Ok(new
            {
                totalPatients,
                totalAppointments,
                appointmentsToday,
                genderDistribution,
                topDoctors
            });

        }
    }
}
