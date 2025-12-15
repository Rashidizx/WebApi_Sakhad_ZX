using Microsoft.AspNetCore.Mvc;
using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX.Controllers
{
    [ApiController]
    [Route("Login/[controller]")]
    public class SendUserPass : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(int CenterId)
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
       ?? HttpContext.Connection.LocalIpAddress?.ToString();

            LoginResponse response = new LoginResponse()
            {
                data = null,
                message = "",
                status = -1
            };

            try
            {
                MainClassStatic.FnAddCenter(CenterId);
                var FindedCenter = MainClassStatic.FnGetCenter(CenterId);
                LoginRequest request = new LoginRequest
                {
                    username = FindedCenter.UserName,
                    password = FindedCenter.Password,
                    cid = CenterId
                };
                PopularStaticClass.CreateHeadersList(CenterId);

                response = await CallWebSevice.FnLoginAsync(request, FindedCenter);

                if (PopularStaticClass.ChechStatus(response))
                {
                    if (response.data != null && response.data.Count > 0)
                    {
                        PopularStaticClass.FnSetHeaders(response.data[0].sessionId, response.data[0].requestId, null, FindedCenter);
                    }
                    else
                    {
                        response.message = "";
                        response.status = -1;
                    }
                }
                else
                {
                    response.message = "";
                    response.status = -1;
                }
            }
            catch (Exception zx)
            {
                zx.Log();
                response.message = "";
                response.status = -1;
            }

            return response;
        }
    }
}