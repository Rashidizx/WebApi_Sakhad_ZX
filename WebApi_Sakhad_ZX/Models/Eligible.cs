namespace WebApi_Sakhad_ZX.Models
{
    /// <summary>
    ///  Eligible ارسال استعلام اطلاعات هویتی و بیمه ای
    /// </summary>
    public class EligibleRequest
    {
        public string nationalNumber { get; set; }
    }

    /// <summary>
    ///  Eligible دریافت پاسخ استعلام اطلاعات هویتی و بیمه ای
    /// </summary>
    public class EligibleResponse
    {
        public int status { get; set; }
        public string message { get; set; }

        public List<Inner_EligibleResponse> data { get; set; }
    }

    public class Inner_EligibleResponse
    {
        public List<Disease> diseases { get; set; } = new List<Disease>();

        public string nationalNumber { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string BirthDate { get; set; }

        public int Gender { get; set; }

        public string RelationType { get; set; }

        public int RelationTypeCode { get; set; }

        public string Mobile { get; set; }

        /// <summary>
        /// وضعیت استحقاق بیمه ای
        /// 1 - فعال
        /// 0 - غیرفعال
        /// 2 - غیرفعال
        /// </summary>
        public int IsCovered { get; set; }

        public string IsCoveredTitle { get; set; }

        public int EsarStatusType { get; set; }

        public string EsarStatusTitle { get; set; }

        public string Age { get; set; }

        public string Address { get; set; }

        public int? InsuranceNumberSarparast { get; set; }

        public string nationalNumberSarparast { get; set; }

        public string NameFamilySarparast { get; set; }

        public string MemberImage { get; set; }
    }

    /// <summary>
    /// پرونده های بیماری بیمه شده
    /// </summary>
    public class Disease
    {
        /// <summary>
        /// Description of the disease
        /// </summary>
        public string DiseasesDesc { get; set; }

        /// <summary>
        /// Date until which the disease is considered valid (format as string)
        /// </summary>
        public string DiseasesValidDate { get; set; }

        /// <summary>
        /// Disease code (classification code)
        /// </summary>
        public string DiseasesCode { get; set; }
    }
}