using Microsoft.AspNetCore.Mvc;
using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX.Controllers
{
    [ApiController]
    [Route("Login/[controller]")]
    public class SendUserPass : ControllerBase
    {
        [HttpPost("Login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
       ?? HttpContext.Connection.LocalIpAddress?.ToString();

            return new LoginResponse()
            {
                data = new List<Inner_LoginResponse>() { },
                message = "ok",
                status = 2000
            };
        }
    }
}