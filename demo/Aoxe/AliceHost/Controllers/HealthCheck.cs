using Microsoft.AspNetCore.Mvc;

namespace AliceHost.Controllers
{
    [Route("[Controller]")]
    public class HealthCheckController : Controller
    {
        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}