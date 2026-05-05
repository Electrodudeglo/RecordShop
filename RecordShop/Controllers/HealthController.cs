using Microsoft.AspNetCore.Mvc;

namespace RecordShop.Controllers
{
    [ApiController]
    [Route("[Controller]")]
      public class HealthController : ControllerBase
    {

        [Route("/health")]
        public IActionResult Index()
        {
            return Ok("Health");
        }
    }
}
