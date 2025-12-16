using Microsoft.AspNetCore.Mvc;
using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX.Controllers
{
    [ApiController]
    [Route("PrescribeItemsList/[controller]")]
    public class GetPrescribeItemsList : Controller
    {
        [HttpPost("PrescribeItemsList1")]
        public async Task<ActionResult<getPrescribeItemsListResponse>> PrescribeItemsListAsync([FromBody] getPrescribeItemsListRequest request, int CenterId)
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
       ?? HttpContext.Connection.LocalIpAddress?.ToString();

            getPrescribeItemsListResponse response = new getPrescribeItemsListResponse()
            {
                data = null,
                message = "خطای مقدار دهی اولیه داخلی",
                status = -1
            };

            try
            {
                MainClassStatic.FnAddCenter(CenterId);
                var FindedCenter = MainClassStatic.FnGetCenter(CenterId);

                response = await CallWebSevice.FnGetPrescribeItemsListAsync(request, FindedCenter);

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