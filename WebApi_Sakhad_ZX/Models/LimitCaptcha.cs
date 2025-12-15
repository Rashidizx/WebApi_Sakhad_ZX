namespace WebApi_Sakhad_ZX.Models
{
    #region verifyCaptcha

    /// <summary>
    ///  verifyCaptcha ارسال به کپچا برای رفع محدودیت دسترسی به سرویس ها
    /// </summary>
    public class verifyCaptchaRequest
    {
        public string answer { get; set; }
    }

    /// <summary>
    ///  verifyCaptcha دریافت  پاسخ به کپچا برای رفع محدودیت دسترسی به سرویس ها
    /// </summary>
    public class verifyCaptchaResponse
    {
        public int status { get; set; }
        public string message { get; set; }

        public List<Inner_verifyCaptchaResponse> data { get; set; }
    }

    public class Inner_verifyCaptchaResponse
    {
        public bool result { get; set; }
    }

    #endregion verifyCaptcha
}