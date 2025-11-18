namespace WebApi_Sakhad_ZX.Models
{
    /// <summary>
    ///  ورود به سامانه Login
    /// </summary>
    public class LoginRequest
    {
        public string username { get; set; }

        public string password { get; set; }
        public int cid { get; set; }
    }

    /// <summary>
    ///  دریافت پاسخ ورود به سامانه Login
    /// </summary>
    public class LoginResponse
    {
        public int status { get; set; }
        public string message { get; set; }

        public List<Inner_LoginResponse> data { get; set; }
    }

    public class Inner_LoginResponse
    {
        public string captcha { get; set; }
        public string sessionId { get; set; }
        public string requestId { get; set; }
    }
}