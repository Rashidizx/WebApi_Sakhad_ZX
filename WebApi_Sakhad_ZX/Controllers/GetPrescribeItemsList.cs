using Microsoft.AspNetCore.Mvc;
using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX.Controllers
{
    [ApiController]
    [Route("PrescribeItemsList/[controller]")]
    public class GetPrescribeItemsList : Controller
    {
        [HttpPost("PrescribeItemsList")]
        public ActionResult<getPrescribeItemsListResponse> PrescribeItemsList([FromBody] getPrescribeItemsListRequest request)
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
       ?? HttpContext.Connection.LocalIpAddress?.ToString();

            return new getPrescribeItemsListResponse()
            {
                data =  new List<Inner_getPrescribeItemsListResponse>() { },
                message = "ok",
                status = 2000
            };
        }
    }
}
