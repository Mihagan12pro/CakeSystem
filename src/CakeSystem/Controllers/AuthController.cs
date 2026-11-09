using Microsoft.AspNetCore.Mvc;

namespace CakeSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IHostEnvironment _env;

        [HttpGet("signup")]
        public IActionResult GetSignUp()
        {
            var filePath = Path.Combine(new DirectoryInfo(_env.ContentRootPath).Parent.Parent.FullName, "Public", "SignUp.html");

            return PhysicalFile(filePath, "text/html");
        }


        public AuthController(IHostEnvironment env)
        {
            _env = env;
        }
    }
}
