namespace WebApi_Sakhad_ZX
{
    public class SakhadCenter
    {
        /// <summary>
        /// لیست همه هدر های وب سرویس ها
        /// </summary>
        public Dictionary<string, HttpRequestMessage> AllHeadersByURL = new Dictionary<string, HttpRequestMessage>();

        public string UserName = "";
        public string Password = "";
        public string Mobile = "";
        public int CenterId = -1;
        public string Workstationid = "";
        public string ClientSecret = "";
        public string ClientId = "";

        /// <summary>
        /// کد نوع تجویز مطابق جدول 303
        /// </summary>
        public string type303 { get; set; }

        public string SessionId = "test";
        public string RequestId = "test";
        public string ExpireSessionId = "test";
        public string AccessToken = "test";
        public string ExpireAccessToken = "test";
        public string NoMedicalSystem = "test";
        private string _Authorization = "test";

        /// <summary>
        ///توکن دریافت شده در سرویس ورود مرحله 2 به صورت Bearer {token}
        ///نکته باحال همون اسم دیگه  accessToken
        /// </summary>
        public string Authorization
        {
            get
            {
                return "Bearer " + _Authorization;
            }
            set
            {
                _Authorization = value; // مقدار جدید را ذخیره کن
                PopularStaticClass.CreateHeadersList(CenterId); // تابع تغییر هدر ها رو صدا بزن
            }
        }

        public SakhadCenter(InitilizerCenter _InitCenter)
        {
            CenterId = _InitCenter.CenterId;
            UserName = _InitCenter.UserName;
            Password = _InitCenter.Password;
            Workstationid = _InitCenter.Workstationid;
            ClientSecret = _InitCenter.ClientSecret;
            ClientId = _InitCenter.ClientId;
            Mobile = _InitCenter.Mobile;
        }
    }

    /// <summary>
    /// کلاس مقدار دهی اولیه برای لوگین مراکز
    /// </summary>
    public class InitilizerCenter
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int CenterId { get; set; }

        /// <summary>
        /// یوزر نیم کاربر
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// پسورد کاربر
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// شناسه منحصر به فرد کلاینت متصل شده

        /// </summary>
        public string Workstationid { get; set; }

        /// <summary>
        /// کلاینت سکرت
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// کلاینت آیدی
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// موبایل کاربر برای ورود دو مرحله ای
        /// </summary>
        public string Mobile { get; set; }
    }
}