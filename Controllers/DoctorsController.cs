using ClinicAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [Tags("3. Doctors")]
    [ApiController]
    [Route("api/doctors")]
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            var doctors = _context.Users
                .Where(u => u.Role == "Doctor")
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email
                })
                .ToList();

            return Ok(doctors);
        }
    }
}
