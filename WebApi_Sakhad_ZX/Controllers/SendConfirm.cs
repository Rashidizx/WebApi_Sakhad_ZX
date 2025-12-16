using Microsoft.AspNetCore.Mvc;
using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX.Controllers
{
    [ApiController]
    [Route("Confirm/[controller]")]
    public class SendConfirm : Controller
    {
        [HttpPost("Confirm")]
        public async Task<ActionResult<ConfirmResponse>> ConfirmAsync(string CaptchaAnswer, int CenterId)
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
       ?? HttpContext.Connection.LocalIpAddress?.ToString();
            PopularStaticClass.CreateHeadersList(CenterId);

            ConfirmResponse response = new ConfirmResponse()
            {
                data = null,
                message = "خطای مقدار دهی اولیه داخلی",
                status = -1
            };

            try
            {
                var FindedCenter = MainClassStatic.FnGetCenter(CenterId);
                var request = new ConfirmRequest
                {
                    answer = CaptchaAnswer,
                    sessionId = FindedCenter.SessionId
                };

                response = await CallWebSevice.FnConfirmAsync(request, FindedCenter);

                if (PopularStaticClass.ChechStatus(response))
                {
                    if (response.data != null && response.data.Count > 0)
                    {
                        PopularStaticClass.FnSetHeaders(response.data[0].sessionId, response.data[0].requestId, response.data[0].expireSessionId, FindedCenter);
                        PopularStaticClass.FnSetHeaders(response.data[0].accessToken, response.data[0].expireAccessToken, FindedCenter);
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