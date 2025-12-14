using Microsoft.AspNetCore.Mvc;
using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX.Controllers
{
    [ApiController]
    [Route("Eligible/[controller]")]
    public class GetEligible : Controller
    {
        [HttpPost("Eligible")]
        public ActionResult<EligibleResponse> Eligible([FromBody] EligibleRequest request)
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
       ?? HttpContext.Connection.LocalIpAddress?.ToString();

            return new EligibleResponse()
            {
                data = new List<Inner_EligibleResponse>() { },
                message = "ok",
                status = 2000
            };
        }
    }
}
