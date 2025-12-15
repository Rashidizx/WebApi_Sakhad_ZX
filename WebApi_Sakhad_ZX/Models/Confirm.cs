namespace WebApi_Sakhad_ZX.Models
{
    /// <summary>
    ///  confirm ارسال تایید رمز ثابت
    /// </summary>
    public class ConfirmRequest
    {
        //public string MiniUrl { get { return Sakhad_StaticInfoURL.Url_confirm; } }
        public string sessionId { get; set; }

        public string answer { get; set; }
    }

    /// <summary>
    ///  confirm دریافت پاسخ تایید رمز ثابت
    /// </summary>
    public class ConfirmResponse
    {
        public int status { get; set; }
        public string message { get; set; }

        public List<Inner_ConfirmResponse> data { get; set; }
    }

    public class Inner_ConfirmResponse
    {
        public string requestId { get; set; }
        public string accessToken { get; set; }

        public string sessionId { get; set; }
        public string expireAccessToken { get; set; }

        public string expireSessionId { get; set; }
    }
}