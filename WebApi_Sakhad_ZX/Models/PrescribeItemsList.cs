namespace WebApi_Sakhad_ZX.Models
{
    /// <summary>
    ///  getPrescribeItemsList ارسال واکشی اقلام تجویزی پزشک در نسخه پیچی الکترونیک
    /// </summary>
    public class getPrescribeItemsListRequest
    {
        /// <summary>
        /// کد ملی بیمار
        /// </summary>
        public string nationalNumber { get; set; }

        /// <summary>
        /// کد رهگیری نسخه تجویز ی
        /// </summary>
        public string trackingCode { get; set; }

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

    /// <summary>
    ///  getPrescribeItemsList دریافت پاسخ واکشی اقلام تجویزی پزشک در نسخه پیچی الکترونیک
    /// </summary>
    public class getPrescribeItemsListResponse
    {
        /// <summary>
        /// کد نتیجه اجرای وب سرویس است
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// پیغام نتیجه اجرای وب سرویس است
        /// </summary>
        public string message { get; set; }

        public List<Inner_getPrescribeItemsListResponse> data { get; set; }
    }

    public class Inner_getPrescribeItemsListResponse
    {
        /// <summary>
        /// نام مرکز تجویزکننده
        /// </summary>
        public string centerName { get; set; }

        /// <summary>
        /// نام پزشک تجویزکننده
        /// </summary>
        public string doctorName { get; set; }

        /// <summary>
        /// کد رهگیری نسخه تجویزی
        /// </summary>
        public string trackingCode { get; set; }

        /// <summary>
        /// متن پیام
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// اطلاعات تجویزکننده شامل:
        /// شرح رشته تحصیلی )تخصص(، مقطع تحصیلی و وضعیت هیئت علمی
        /// </summary>
        public string doctorSpecialty { get; set; }

        /// <summary>
        /// کد استحقاق بیمه ای نسخه تجویزی )جهت انجام خدمت)
        /// مهم: این کد به ازای هر نسخهی ثبت شده )ثبت نهایی( تولید شده و در اختیار قرار می گیرد. ذکر این نکته که، این مقدار هیچ ارتباطی با خروجی سرویس استحقاق سنجی ) eligibleHid ( نداشته و به صورت مستقل و در زمان ثبت نسخه نویسی تولید می شود.
        /// </summary>
        public string Hid { get; set; }

        /// <summary>
        /// کد شکایت اصلی بیمار
        /// </summary>
        public string patientComplaintCode { get; set; }

        /// <summary>
        /// شرح شکایت اصلی بیمار
        /// </summary>
        public string patientComplaintDesc { get; set; }

        /// <summary>
        /// کد ملی بیمار
        /// </summary>
        public string nationalNumber { get; set; }

        /// <summary>
        /// تاریخ تجویز نسخه
        /// </summary>
        public string prescriptionDate { get; set; }

        /// <summary>
        ///  شماره نظام پزشکی یا کد عضو
        ///  مهم: همان مقدار درج شده در پارامتر ورودی برگردانده می شود .
        /// </summary>
        public string nomedicalSystem { get; set; }

        /// <summary>
        /// وضعیت شماره نظام/کد عضو وارد شده در پارامتر ورودی:
        /// 1 - عضو نظام پزشکی 2 - غیر عضو نظام پزشکی
        /// </summary>
        public string medicalSystemType { get; set; }

        /// <summary>
        ///شرح ابطال نسخه
        ///مهم: چنانچه به هر دلیل )از جمله ضوابط و تعهدات ساخد( نسخه ویزیت ابطال شود شرح ابطال نسخه در این پارامتر نمایش داده می شود .
        /// </summary>
        public string cancellation { get; set; }

        /// <summary>
        ///  کد ابطال نسخه
        ///  مهم: چنانچه به هر دلیل )از جمله ضوابط و تعهدات ساخد( نسخه ویزیت ابطال شود کد علت ابطال نسخه در این پارامتر نمایش داده می شود
        /// </summary>
        public string cancellationCode { get; set; }

        public List<DetailList> detailList { get; set; } = new List<DetailList>();
    }    /// <summary>

         /// چنانچه از اقلام تجویزی پزشک، مواردی ارائه شده باشند در این لیست با ذکر نام مرکز، تعداد دریافتی و ...  .
         /// </summary>
    public class DetailList
    {
        /// <summary>
        /// ترمینولوژی خدمت تجویز ی
        /// </summary>
        public string terminology { get; set; }

        /// <summary>
        /// کد ملی خدم ت
        //مهم: کد خدمت بر اساس استاندارد تعیین شده
        //و با توجه به ترمینولوژی واکشی می شود
        /// </summary>
        public int code { get; set; }

        /// <summary>
        ///  کد نوع تجویز
        ///  مهم: این کد در پارامتر ورودی ) Type ( نیز
        ///  ارسال شده بود. )مطابق جدول 303
        /// </summary>
        public string prescriptionTypeCode { get; set; }

        /// <summary>
        /// نام خدم ت
        /// </summary>
        public string fullName { get; set; }

        /// <summary>
        /// تعداد تجویز
        /// </summary>
        public int prescribeCount { get; set; }

        /// <summary>
        /// تعداد ارائه شده
        ///چنانچه خدمت مورد نظر در موسسات ارائه دهنده خدمت )داروخانه، آزمایشگاه، تصویربرداری و...(ارائه شده باشد میزان ارائه در این پارامتر مشخص می شود.به طور مثال از طریق این پارامتر میتوان فهمید بیمار، داروی خود را در داروخانه دریافت کرده یا خیر.
        /// </summary>
        public double? presentedCount { get; set; }

        /// <summary>
        /// زمان مصر ف
        /// مهم: جهت اطلاع از کدهای زمان مصرف از سرویس زمان مصرف ( 3 - 32 ) استفاده شود.
        /// </summary>
        public string consumption { get; set; }

        /// <summary>
        /// شرح زمان مصر ف
        /// </summary>
        public string consumptionDesc { get; set; }

        /// <summary>
        ///     میزان مصر ف
        ///     مهم: جهت اطلاع از کدهای میزان مصرف از سرویس میزان مصرف ) 3 - 33 ( استفاده شود.
        /// </summary>
        public string consumptionInstruction { get; set; }

        /// <summary>
        /// شرح میزان مصر ف
        /// </summary>
        public string consumptionInstructionDesc { get; set; }

        /// <summary>
        /// اگر true باشد به معنای انجام اورژانسی و در غیر اینصورت به معنای اورژانسی نبودنِ انجام خدمت می باشد. تعیین مقدار این پارامتر، با پزشک تجویزکننده می باشد
        /// </summary>
        public double? emergency { get; set; }

        /// <summary>
        ///  توضیحات
        ///  مهم: توضیحات اختصاصی پزشک برای مراکز ارائه دهنده خدمت یا توضیحات مربوط به آمادگی های قبل از انجام خدمت )برای بیمار( در این پارامتر واکشی می شود .
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// تاریخ قلم نسخه
        /// مهم: تاریخ تجویز پزشک در این پارامتر واکشی می شود و از تاریخ موردنظر امکان ارائه نیز وجود دارد. چنانچه این تاریخ بزرگتر از تاریخ جاری باشد به عنوان نسخه تاریخ آینده محسوب شده و اجازه ارائه آن خدمت تا زمان مقرر وجود ندارد
        /// </summary>
        public string creationDate { get; set; }

        /// <summary>
        /// شناسه ریز نسخه
        /// رشته UUID  36 کار ا کتری
        /// </summary>
        public string prescribeServiceUuid { get; set; }

        /// <summary>
        /// شماره داروی ترکیبی
        /// </summary>
        public double? bulkId { get; set; }

        /// <summary>
        /// 1 -می باشد
        /// 2 -نمی باشد
        /// </summary>
        public string otc { get; set; }

        /// <summary>
        /// نیازمند اصالت سنجی
        ///  1 - میباشد
        ///  2 - نمیباشد
        ///  مهم: چنانچه خدمت ارائه شده جزو اقلام تجویزی پزشک در نسخه نباشد و به درخواست بیمار ارائه شود با مقدار 1 و در غیر اینصورت با مقدار 2 مشخص میشود. ذکر این نکته که، اقلامی که با مقدار 1 ارسال می شوند به صورت آزاد محاسبه شده و توسط بیمار پرداخت می گردد.
        /// مهم: چنانچه اصالت سنجی یک خدمت در زمان ارائه )توسط مراجع ذینفع( الزامی باشد این پارامتر با مقدار 1 برگردانده می شود. مثلا اصالت سنجی برای pen انسولی ن
        /// </summary>
        public string shouldSendToEsalat { get; set; }

        /// <summary>
        /// چنانچه از اقلام تجویزی پزشک، مواردی ارائه شده باشند در این لیست با ذکر نام مرکز، تعداد دریافتی و .... واکشی می شود.
        /// </summary>
        public List<InnerDetailList> innerDetailList = new List<InnerDetailList>();
    }

    /// <summary>
    /// چنانچه از اقلام تجویزی پزشک، مواردی ارائه شده باشند در این لیست با ذکر نام مرکز، تعداد دریافتی و .... واکشی می شود
    /// </summary>
    public class InnerDetailList
    {/// <summary>
     /// کد خدمت ارائه شده
     /// </summary>
        public string code { get; set; }

        /// <summary>
        /// تعداد ارائه شده
        /// </summary>
        public int presentedCount { get; set; }

        /// <summary>
        /// نام خدمت
        /// </summary>
        public string fullName { get; set; }

        /// <summary>
        /// کد زمان
        ///  مصرف مهم: جهت اطلاع از کدهای زمان مصرف از سرویس زمان مصرف ( 3 - 32 ) استفاده شود.
        /// </summary>
        public string consumption { get; set; }

        /// <summary>
        /// شرح زمان مصرف
        /// </summary>
        public string consumptionDesc { get; set; }

        /// <summary>
        /// میزان مصرف
        ///  مهم: جهت اطلاع از کدهای میزان مصرف از سرویس میزان مصرف (3 - 33 ) استفاده شود.
        /// </summary>
        public string consumptionInstruction { get; set; }

        /// <summary>
        /// شرح میزان
        /// </summary>            public string consumptionInstructionDesc { get; set; }
        public string consumptionInstructionDesc { get; set; }

        /// <summary>
        /// توضیحات
        ///  مهم: توضیحاتی که ارائه دهنده برای آن خدمت درج می کند
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// نام مرکز ارائه دهنده
        /// </summary>
        public string centerName { get; set; }
    }
}