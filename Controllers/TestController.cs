using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [Tags("7. Test")]
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
