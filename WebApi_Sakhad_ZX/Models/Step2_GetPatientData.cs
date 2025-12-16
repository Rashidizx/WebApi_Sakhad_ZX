namespace WebApi_Sakhad_ZX.Models
{
    public class Step2_GetPatientDataRequest
    {
        public string CaptchaAnswer { get; set; }
        public int CenterId { get; set; }

        /// <summary>
        /// کد ملی بیمار
        /// </summary>
        public string nationalNumber { get; set; }

        /// <summary>
        /// کد رهگیری نسخه تجویز ی
        /// </summary>
        public string trackingCode { get; set; }
    }

    public class Step2_GetPatientDataResponse
    {
        public int status { get; set; }
        public string message { get; set; }

        public InnerStep2_GetPatientDataResponse data { get; set; }
    }

    public class InnerStep2_GetPatientDataResponse
    {
        public string nationalNumber { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string BirthDate { get; set; }

        public string bimehEndDate { get; set; }

        /// <summary>
        /// وضعیت استحقاق بیمه ای
        /// 1 - فعال
        /// 0 - غیرفعال
        /// 2 - غیرفعال
        /// </summary>
        public int IsCovered { get; set; }
        public List<Inner_getPrescribeItemsListResponse>? PrescribeItemsList { get; set; }
    }
}