using Microsoft.AspNetCore.Mvc;
using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX.Controllers
{
    [ApiController]
    [Route("Step1_Login/[controller]")]
    public class Step1_Login : ControllerBase
    {
        /// <summary>
        /// متد ورود به ساخد
        /// </summary>
        /// <param name="CenterId">کد مرکز</param>
        /// <param name="type303">کد مرکز بر اساس جدول 303</param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ActionResult<Step1_LoginResponse>> LoginAsync([FromBody] Step1_LoginRequest ZxRequest)
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
       ?? HttpContext.Connection.LocalIpAddress?.ToString();

            Step1_LoginResponse response = new Step1_LoginResponse()
            {
                captchaBase64 = null,
                message = "خطای مقدار دهی اولیه داخلی",
                status = -1
            };

            try
            {
                MainClassStatic.FnAddCenter(ZxRequest.CenterId);
                var FindedCenter = MainClassStatic.FnGetCenter(ZxRequest.CenterId);
                FindedCenter.type303 = ZxRequest.type303;
                LoginRequest request = new LoginRequest
                {
                    username = FindedCenter.UserName,
                    password = FindedCenter.Password,
                    cid = ZxRequest.CenterId
                };
                PopularStaticClass.CreateHeadersList(ZxRequest.CenterId);

                var loginResponse = await CallWebSevice.FnLoginAsync(request, FindedCenter);

                if (PopularStaticClass.ChechStatus(loginResponse))
                {
                    if (loginResponse.data != null && loginResponse.data.Count > 0)
                    {
                        PopularStaticClass.FnSetHeaders(loginResponse.data[0].sessionId, loginResponse.data[0].requestId, null, FindedCenter);
                        response.captchaBase64 = loginResponse.data[0].captcha;
                        response.message = loginResponse.message;
                        response.status = loginResponse.status;
                    }
                    else
                    {
                        response.message = $"{loginResponse.message}";
                        response.status = -2;
                    }
                }
                else
                {
                    response.message = loginResponse.message;
                    response.status = loginResponse.status;
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