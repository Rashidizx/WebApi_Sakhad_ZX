using Microsoft.AspNetCore.Mvc;
using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX.Controllers
{
    [ApiController]
    [Route("Eligible/[controller]")]
    public class GetEligible : Controller
    {
        [HttpPost("Eligible")]
        public async Task<ActionResult<EligibleResponse>> EligibleAsync(string nationalNumber, int CenterId)
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
       ?? HttpContext.Connection.LocalIpAddress?.ToString();

            EligibleResponse response = new EligibleResponse()
            {
                data = null,
                message = "خطای مقدار دهی اولیه داخلی",
                status = -1
            };

            try
            {
                MainClassStatic.FnAddCenter(CenterId);
                var FindedCenter = MainClassStatic.FnGetCenter(CenterId);
                EligibleRequest request = new EligibleRequest
                {
                    nationalNumber = nationalNumber
                };

                response = await CallWebSevice.FnEligibleAsync(request, FindedCenter);

                if (PopularStaticClass.ChechStatus(response))
                {
                    if (response.data != null && response.data.Count > 0)
                    {
                        //fix me use data
                    }
                    else
                    {
                        response.message = $"{response.message}";
                        response.status = -2;
                    }
                }
                else
                {
                    response.message = response.message;
                    response.status = response.status;
                }
            }
            catch (Exception zx)
            {
                zx.Log();
                response.message = $"خطا داخلی وب سرویس علوم پزشکی:{zx.Message}";
                response.status = -10;
            }

            return response;
        }
    }
}