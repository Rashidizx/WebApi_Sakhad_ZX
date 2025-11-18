namespace WebApi_Sakhad_ZX.Models
{
    /// <summary>
    ///  GetCaptcha ارسال مجدد کپچا
    /// </summary>
    public class GetCaptchaRequest
    {
        //public string MiniUrl { get { return Sakhad_StaticInfoURL.Url_getCaptcha; } }
        public string sessionId { get; set; }
    }

    /// <summary>
    ///  GetCaptcha دریافت پاسخ ارسال مجدد کپچا
    /// </summary>
    public class GetCaptchaResponse
    {
        public int status { get; set; }
        public string message { get; set; }

        public List<Inner_GetCaptchaResponse> data { get; set; }
    }

    public class Inner_GetCaptchaResponse
    {
        public string captcha { get; set; }
        public string requestId { get; set; }
        public string sessionId { get; set; }
    }
}