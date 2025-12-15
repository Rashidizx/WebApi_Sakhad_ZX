using Newtonsoft.Json;

namespace WebApi_Sakhad_ZX.Models
{
    /// <summary>
    ///  پاسخ دریافتی وسط کار وب سرویس برای استفاده از وب سرویس رفع محدودیت با کپچا
    /// </summary>
    public class UnlockByCaptchaVerificationResponse
    {
        public int status { get; set; }
        public string message { get; set; }

        [JsonProperty("data")]
        public List<Inner_UnlockByCaptchaVerificationResponse> data { get; set; }
    }

    public class Inner_UnlockByCaptchaVerificationResponse
    {
        public string captcha { get; set; }
    }
}