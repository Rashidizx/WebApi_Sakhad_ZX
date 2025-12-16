namespace WebApi_Sakhad_ZX.Models
{
    public class Step1_LoginRequest
    {
        public int CenterId { get; set; }
        public string type303 { get; set; }
    }

    public class Step1_LoginResponse
    {
        public int status { get; set; }
        public string message { get; set; }

        public string captchaBase64 { get; set; }
    }
}