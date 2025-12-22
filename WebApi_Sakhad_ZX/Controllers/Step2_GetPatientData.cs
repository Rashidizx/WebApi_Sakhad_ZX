using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX.Controllers
{
    [ApiController]
    [Route("Step2_GetPatientDataRequestn/[controller]")]
    public class Step2_GetPatientData : Controller
    {
        [HttpPost("GetData")]
        public async Task<ActionResult<Step2_GetPatientDataResponse>> ConfirmAsync([FromBody] Step2_GetPatientDataRequest ZxRequest)
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
       ?? HttpContext.Connection.LocalIpAddress?.ToString();

            Step2_GetPatientDataResponse finalResponse = new Step2_GetPatientDataResponse()
            {
                data = new InnerStep2_GetPatientDataResponse(),
                message = "خطای مقدار دهی اولیه داخلی",
                status = -1
            };

            try
            {
                if (string.IsNullOrEmpty(ZxRequest.CaptchaAnswer.Trim()))
                {
                    var FindedCenter = MainClassStatic.FnGetCenter(ZxRequest.CenterId);
                    var request = new ConfirmRequest
                    {
                        answer = ZxRequest.CaptchaAnswer,
                        sessionId = FindedCenter.SessionId
                    };


                    EligibleResponse EliResponse = new EligibleResponse()
                    {
                        data = null,
                        message = "خطای مقدار دهی اولیه داخلی",
                        status = -2
                    };

                    try
                    {
                        //  MainClassStatic.FnAddCenter(ZxRequest.CenterId);
                        FindedCenter = MainClassStatic.FnGetCenter(ZxRequest.CenterId);
                        EligibleRequest EliRequest = new EligibleRequest
                        {
                            nationalNumber = ZxRequest.nationalNumber
                        };

                        EliResponse = await CallWebSevice.FnEligibleAsync(EliRequest, FindedCenter);

                        if (PopularStaticClass.ChechStatus(EliResponse))
                        {
                            if (EliResponse.data != null && EliResponse.data.Count > 0 && EliResponse.data.FirstOrDefault() != null)
                            {
                                var Eli = EliResponse.data.FirstOrDefault();

                                finalResponse.data.nationalNumber = Eli.nationalNumber;
                                finalResponse.data.Name = Eli.Name;
                                finalResponse.data.LastName = Eli.LastName;
                                finalResponse.data.BirthDate = Eli.BirthDate;
                                finalResponse.data.bimehEndDate = null;//تاریخ انقضا بیمه نداریم
                                finalResponse.data.IsCovered = Eli.IsCovered;
                                getPrescribeItemsListResponse PrescribeResponse = new getPrescribeItemsListResponse()
                                {
                                    data = new List<Inner_getPrescribeItemsListResponse>(),
                                    message = "خطای مقدار دهی اولیه داخلی",
                                    status = -3
                                };

                                try
                                {
                                    FindedCenter = MainClassStatic.FnGetCenter(ZxRequest.CenterId);

                                    getPrescribeItemsListRequest PrescribeRequest = new getPrescribeItemsListRequest
                                    {
                                        nationalNumber = ZxRequest.nationalNumber,
                                        trackingCode = ZxRequest.trackingCode,
                                        type = FindedCenter.type303,
                                        orderType = "2"
                                    };
                                    PrescribeResponse = await CallWebSevice.FnGetPrescribeItemsListAsync(PrescribeRequest, FindedCenter);

                                    if (PopularStaticClass.ChechStatus(PrescribeResponse))
                                    {
                                        if (PrescribeResponse.data != null && PrescribeResponse.data.Count > 0 & PrescribeResponse.data.FirstOrDefault() != null)
                                        {
                                            finalResponse.data.PrescribeItemsList = PrescribeResponse.data;
                                        }
                                        else
                                        {
                                            finalResponse.message = $"{PrescribeResponse.message}";
                                            finalResponse.status = -4;
                                        }
                                    }
                                    else
                                    {
                                        finalResponse.message = $"کد رهگیری نسخه یافت نشد {PrescribeResponse.message}";
                                        finalResponse.status = PrescribeResponse.status;
                                    }
                                }
                                catch (Exception zx)
                                {
                                    zx.Log();
                                    finalResponse.message = $"خطا داخلی وب سرویس علوم پزشکی:{zx.Message}";
                                    finalResponse.status = -5;
                                    return finalResponse;
                                }
                            }
                            else
                            {
                                finalResponse.message = $"{EliResponse.message}";
                                finalResponse.status = -6;
                            }
                        }
                        else
                        {
                            finalResponse.message = $"کد ملی یافت نشد {EliResponse.message}";
                            finalResponse.status = EliResponse.status;
                        }
                    }
                    catch (Exception zx)
                    {
                        zx.Log();
                        finalResponse.message = $"خطا داخلی وب سرویس علوم پزشکی:{zx.Message}";
                        finalResponse.status = -7;
                        return finalResponse;
                    }


                }
                else
                {
                    var FindedCenter = MainClassStatic.FnGetCenter(ZxRequest.CenterId);
                    var request = new ConfirmRequest
                    {
                        answer = ZxRequest.CaptchaAnswer,
                        sessionId = FindedCenter.SessionId
                    };

                    var confirmResponse = await CallWebSevice.FnConfirmAsync(request, FindedCenter);

                    if (PopularStaticClass.ChechStatus(confirmResponse))
                    {
                        if (confirmResponse.data != null && confirmResponse.data.Count > 0)
                        {
                            PopularStaticClass.FnSetHeaders(confirmResponse.data[0].sessionId, confirmResponse.data[0].requestId, confirmResponse.data[0].expireSessionId, FindedCenter);
                            PopularStaticClass.FnSetHeaders(confirmResponse.data[0].accessToken, confirmResponse.data[0].expireAccessToken, FindedCenter);
                            PopularStaticClass.CreateHeadersList(ZxRequest.CenterId);
                            EligibleResponse EliResponse = new EligibleResponse()
                            {
                                data = null,
                                message = "خطای مقدار دهی اولیه داخلی",
                                status = -2
                            };

                            try
                            {
                                //  MainClassStatic.FnAddCenter(ZxRequest.CenterId);
                                FindedCenter = MainClassStatic.FnGetCenter(ZxRequest.CenterId);
                                EligibleRequest EliRequest = new EligibleRequest
                                {
                                    nationalNumber = ZxRequest.nationalNumber
                                };

                                EliResponse = await CallWebSevice.FnEligibleAsync(EliRequest, FindedCenter);

                                if (PopularStaticClass.ChechStatus(EliResponse))
                                {
                                    if (EliResponse.data != null && EliResponse.data.Count > 0 && EliResponse.data.FirstOrDefault() != null)
                                    {
                                        var Eli = EliResponse.data.FirstOrDefault();

                                        finalResponse.data.nationalNumber = Eli.nationalNumber;
                                        finalResponse.data.Name = Eli.Name;
                                        finalResponse.data.LastName = Eli.LastName;
                                        finalResponse.data.BirthDate = Eli.BirthDate;
                                        finalResponse.data.bimehEndDate = null;//تاریخ انقضا بیمه نداریم
                                        finalResponse.data.IsCovered = Eli.IsCovered;
                                        getPrescribeItemsListResponse PrescribeResponse = new getPrescribeItemsListResponse()
                                        {
                                            data = new List<Inner_getPrescribeItemsListResponse>(),
                                            message = "خطای مقدار دهی اولیه داخلی",
                                            status = -3
                                        };

                                        try
                                        {
                                           
                                            FindedCenter = MainClassStatic.FnGetCenter(ZxRequest.CenterId);

                                            getPrescribeItemsListRequest PrescribeRequest = new getPrescribeItemsListRequest
                                            {
                                                nationalNumber = ZxRequest.nationalNumber,
                                                trackingCode = ZxRequest.trackingCode,
                                                type = FindedCenter.type303,
                                                orderType = "2"
                                            };
                                            PrescribeResponse = await CallWebSevice.FnGetPrescribeItemsListAsync(PrescribeRequest, FindedCenter);

                                            if (PopularStaticClass.ChechStatus(PrescribeResponse))
                                            {
                                                if (PrescribeResponse.data != null && PrescribeResponse.data.Count > 0 & PrescribeResponse.data.FirstOrDefault() != null)
                                                {
                                                    finalResponse.data.PrescribeItemsList = PrescribeResponse.data;
                                                }
                                                else
                                                {
                                                    finalResponse.message = $"{PrescribeResponse.message}";
                                                    finalResponse.status = -4;
                                                }
                                            }
                                            else
                                            {
                                                finalResponse.message = $"کد رهگیری نسخه یافت نشد {PrescribeResponse.message}";
                                                finalResponse.status = PrescribeResponse.status;
                                            }
                                        }
                                        catch (Exception zx)
                                        {
                                            zx.Log();
                                            finalResponse.message = $"خطا داخلی وب سرویس علوم پزشکی:{zx.Message}";
                                            finalResponse.status = -5;
                                            return finalResponse;
                                        }
                                    }
                                    else
                                    {
                                        finalResponse.message = $"{EliResponse.message}";
                                        finalResponse.status = -6;
                                    }
                                }
                                else
                                {
                                    finalResponse.message = $"کد ملی یافت نشد {EliResponse.message}";
                                    finalResponse.status = EliResponse.status;
                                }
                            }
                            catch (Exception zx)
                            {
                                zx.Log();
                                finalResponse.message = $"خطا داخلی وب سرویس علوم پزشکی:{zx.Message}";
                                finalResponse.status = -7;
                                return finalResponse;
                            }
                        }
                        else
                        {
                            finalResponse.message = $"{confirmResponse.message}";
                            finalResponse.status = -8;
                        }
                    }
                    else
                    {
                        finalResponse.message = confirmResponse.message;
                        finalResponse.status = confirmResponse.status;
                    }

                }

            }
            catch (Exception zx)
            {
                zx.Log();
                finalResponse.message = $"خطا داخلی وب سرویس علوم پزشکی:{zx.Message}";
                finalResponse.status = -10;

                return finalResponse;
            }

            return finalResponse;
        }
    }
}