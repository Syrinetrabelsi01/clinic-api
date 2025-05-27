using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            Console.WriteLine("✅ TestController is working");
            return Ok(new { message = "TestController works!" });
        }
    }
}
