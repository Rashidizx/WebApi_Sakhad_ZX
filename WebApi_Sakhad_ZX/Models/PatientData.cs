namespace WebApi_Sakhad_ZX.Models
{
    public class PatientDataRequest
    {
        /// <summary>
        /// کد ملی بیمار
        /// </summary>
        public string nationalNumber { get; set; }

        /// <summary>
        /// کد رهگیری نسخه تجویز ی
        /// </summary>
        public string printCode { get; set; }

        /// <summary>
        /// کد نوع تجویز مطابق جدول 303
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 1 - ارائه 2 - تجویز
        ///مهم: همیشه مقدار 2 )تجویز(درخواست شود.
        /// </summary>
        public string orderType { get; set; } = "2";
    }

    public class PatientDataResponse
    {
    }
}