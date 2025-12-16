using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using static WebApi_Sakhad_ZX.StaticInfo;

namespace WebApi_Sakhad_ZX
{
    /// <summary>
    /// اطلاعات مربوط به کد نوع تجویز - جدول شماره 303
    /// </summary>
    public enum prescriptionTypeCode_303_enum
    {
        [Description("جنریک")]
        Generic = 0,

        [Description("ویزیت")]
        RVU_1 = 1,

        [Description("دارو")]
        ERX_2 = 2,

        [Description("آزمایش")]
        LOINC_3 = 3,

        [Description("پرتو پزشکی")]
        LOINC_4 = 4,

        [Description("توانبخشی/فیزیوتراپی")]
        LOINC_6 = 6,

        [Description("خدمات پزشکی")]
        RVU_7 = 7,

        [Description("دندانپزشکی")]
        SAKHAD_8 = 8,

        [Description("تجهیزات پزشکی/عینک")]
        SAKHAD_9 = 9
    }

    public static class StaticInfo
    {
        private static readonly object _lock = new object();

        private static readonly string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "ErrorLog.txt");

        public enum CenterCodes_enum
        {
            [Description("مطب پزشکی")]
            MedicalClinic = 12208,

            [Description("داروخانه")]
            Pharmacy = 122089,

            [Description("آزمایشگاه")]
            Laboratory = 122090,

            [Description("تصویربرداری")]
            ImagingCenter = 122091,

            [Description("توانبخشی")]
            Rehabilitation = 122092,

            [Description("خدمات پزشکی")]
            MedicalServices = 122093,

            [Description("مطب دندانپزشکی")]
            DentalClinic = 122094,

            [Description("تجهیزات پزشکی و عینک")]
            MedicalEquipmentAndOptics = 122095
        }

        public static class RuntimeContext
        {
            [ThreadStatic]
            private static Dictionary<string, object> _values;

            public static void Set(string key, object value)
            {
                if (_values == null)
                    _values = new Dictionary<string, object>();
                _values[key] = value;
            }

            public static string Dump()
            {
                if (_values == null || _values.Count == 0)
                    return "🔹 No runtime variables captured.";
                var sb = new StringBuilder("🔸 Captured variables:\n");
                foreach (var kvp in _values)
                    sb.AppendLine($"   {kvp.Key} = {kvp.Value ?? "null"}");
                return sb.ToString();
            }
        }

        /// <summary>
        ///   ثبت جزئیات کامل Exception در فایل لاگ با زمان، کلاس، متد، خط، کاربر، IP و StackTrace
        /// </summary>
        /// <param name="ex">شیء Exception جاری</param>
        public static void Log(this Exception ex)
        {
            try
            {
                lock (_lock)
                {
                    string logDir = Path.GetDirectoryName(_logFilePath);
                    if (!Directory.Exists(logDir))
                        Directory.CreateDirectory(logDir);

                    // اطلاعات عمومی
                    string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string appName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? "UnknownApp";
                    string machineName = Environment.MachineName;
                    string userName = Environment.UserName;
                    string ipAddress = GetLocalIPAddress();

                    // جزئیات خطا
                    var frame = new StackTrace(ex, true).GetFrame(0);

                    string methodName = frame?.GetMethod()?.Name ?? "UnknownMethod";
                    string className = frame?.GetMethod()?.DeclaringType?.FullName ?? "UnknownClass";
                    string fileName = frame?.GetFileName() ?? "UnknownFile";
                    int lineNumber = frame?.GetFileLineNumber() ?? -1;

                    // ساخت متن لاگ
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("──────────────────────────────────────────────────────────────");
                    sb.AppendLine($"⏰ زمان: {timeStamp}");
                    sb.AppendLine($"💻 سیستم: {machineName}");
                    sb.AppendLine($"👤 کاربر ویندوز: {userName}");
                    sb.AppendLine($"🌐 IP: {ipAddress}");
                    sb.AppendLine($"📦 برنامه: {appName}");
                    sb.AppendLine($"📂 فایل: {fileName}");
                    sb.AppendLine($"📚 کلاس: {className}");
                    sb.AppendLine($"🔧 متد: {methodName}");
                    sb.AppendLine($"📍 شماره خط: {lineNumber}");
                    sb.AppendLine($"❌ نوع خطا: {ex.GetType().FullName}");
                    sb.AppendLine($"📜 پیام خطا: {ex.Message}");
                    sb.AppendLine($"📎 InnerException: {ex.InnerException?.Message ?? "ندارد"}");
                    sb.AppendLine($"🧵 Thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                    sb.AppendLine($"🔗 StackTrace:\n{ex.StackTrace}");
                    sb.AppendLine(RuntimeContext.Dump());
                    sb.AppendLine("──────────────────────────────────────────────────────────────\n");

                    File.AppendAllText(_logFilePath, sb.ToString(), Encoding.UTF8);
                }
            }
            catch
            {
                // جلوگیری از خطای مجدد هنگام ثبت لاگ
            }
        }

        private static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return ip.ToString();
                }
            }
            catch { }
            return "UnknownIP";
        }

        public static void Log(this object obj, string methodName)
        {
            try
            {
                lock (_lock)
                {
                    string logDir = Path.GetDirectoryName(_logFilePath);
                    if (!Directory.Exists(logDir))
                        Directory.CreateDirectory(logDir);

                    // اطلاعات عمومی
                    string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string appName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? "UnknownApp";
                    string machineName = Environment.MachineName;
                    string userName = Environment.UserName;
                    string ipAddress = GetLocalIPAddress();

                    // ساخت متن لاگ
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("──────────────────────────────────────────────────────────────");
                    sb.AppendLine($"⏰ زمان: {timeStamp}");
                    sb.AppendLine($"💻 سیستم: {machineName}");
                    sb.AppendLine($"👤 کاربر ویندوز: {userName}");
                    sb.AppendLine($"🌐 IP: {ipAddress}");
                    sb.AppendLine($"📦 برنامه: {appName}");
                    sb.AppendLine($"📚 کلاس: inquiryAuthenticityRequest");
                    sb.AppendLine($"🔧 متد: {methodName}");
                    sb.AppendLine($"🧵 Thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}"); ;
                    sb.AppendLine(RuntimeContext.Dump());
                    sb.AppendLine("\n************\n");
                    sb.AppendLine(PopularStaticClass.ConvertClassToJson(obj));
                    sb.AppendLine("──────────────────────────────────────────────────────────────\n");

                    File.AppendAllText(_logFilePath, sb.ToString(), Encoding.UTF8);
                }
            }
            catch
            {
                // جلوگیری از خطای مجدد هنگام ثبت لاگ
            }
        }
    }

    internal class Sakhad_StaticInfoURL
    {
        /// <summary>
        /// متد ساختن آدرس کامل وب سرویس
        /// </summary>
        /// <param name="PartTowURL"> قسمت دوم آدرس که ثابته</param>
        /// <returns></returns>
        public static string GetFullUrl(string PartTowURL, bool HaveMidel = true)
        {
            if (HaveMidel)
            {
                return Url_TestURL + Url_TestURL_midell + PartTowURL;
            }

            return Url_TestURL + PartTowURL;
        }

        /// <summary>
        ///آدرس سرور تست ساخد
        /// </summary>
        public static readonly string Url_TestURL = "https://esakhad.esata.ir:9081/gateway/";

        /// <summary>
        ///آدرس میانی سرور تست ساخد
        /// </summary>
        public static readonly string Url_TestURL_midell = "webApi-test/";

        /// <summary>
        ///آدرس میانی سرور اصلی ساخد
        /// </summary>
        public static readonly string Url_MaintURL_midell = " ";

        /// <summary>
        /// آدرس وب سرویس اصلی ساخد
        /// </summary>
        public static readonly string Url_MainURLt = "";

        /// <summary>
        /// وب سرویس ارسال رمز پویا
        /// </summary>
        public static readonly string Url_send_otp = "auth/send-otp";

        /// <summary>
        /// وب سرویس تایید رمز پویا
        /// </summary>
        public static readonly string Url_verify_otp = "auth/verify-otp";

        /// <summary>
        /// وب سرویس ارسال رمز ثابت
        /// </summary>
        public static readonly string Url_login = "auth/login";

        /// <summary>
        /// وب سرویس تایید رمز ثابت
        /// </summary>
        public static readonly string Url_confirm = "auth/confirm";

        /// <summary>
        /// وب سرویس درخواست مجدد کپچا برای تایید رمز ثابت
        /// </summary>
        public static readonly string Url_getCaptcha = "auth/getCaptcha";

        /// <summary>
        /// وب سرویس دریافت توکن با نام کاربری
        /// </summary>
        public static readonly string Url_getToken = "auth/getToken";

        /// <summary>
        /// وب سرویس پاسخ به کپچا جهت رفع محدودیت
        /// </summary>
        public static readonly string Url_verifyCaptcha = "verifyCaptcha";

        /// <summary>
        /// وب سرویس استعلام اطلاعات هویتی و بیم های
        /// </summary>
        public static readonly string Url_eligible = "v1/eligible";

        /// <summary>
        /// وب سرویس واکشی لیست انواع تخصص
        /// </summary>
        public static readonly string Url_getSpecialityListByName = "v1/getSpecialityListByName";

        /// <summary>
        /// وب سرویس واکشی اطلاعات یک پزشک /مسئول فنی در مرکز جاری
        /// </summary>
        public static readonly string Url_inquiryDoctor = "v1/inquiryDoctor";

        /// <summary>
        /// وب سرویس واکشی اقلام تجویزی پزشک در نسخه پیچی الکترونیک
        /// </summary>
        public static readonly string Url_getPrescribeItemsList = "v1/getPrescribeItemsList";

        /// <summary>
        /// وب سرویس ثبت اولیه نسخه در نسخه پیچی الکترونیک
        /// </summary>
        public static readonly string Url_registerInitialPrescription = "v1/registerInitialPrescription";

        /// <summary>
        /// سرویس تایید نهایی نسخه در نسخه پیچ  (الکترونیکی / کاغذی)
        /// </summary>
        public static readonly string Url_confirmPrescriptionPresentation = "v1/confirmPrescriptionPresentation";

        /// <summary>
        /// وب سرویس حذف نسخه در نسخه پیچی )الکترونیکی / کاغذی (
        /// </summary>
        public static readonly string Url_deletePrescription = "v1/deletePrescription";

        /// <summary>
        ///   وب سرویس استعلام اصالت در نسخه پیچی )الکترونیکی / کاغذی (
        /// </summary>
        public static readonly string Url_inquiryAuthenticity = "v1/inquiryAuthenticity";

        /// <summary>
        ///   وب سرویس تاریخچه ارائه خدمت
        /// </summary>
        public static readonly string Url_getHistoryPresentation = "v1/getHistoryPresentation";

        /// <summary>
        ///   وب سرویس واکشی جزئیات )اقلام( تاریخچه ارائه خدمت
        /// </summary>
        public static readonly string Url_getSubHistoryPresentation = "v1/getSubHistoryPresentation";

        /// <summary>
        ///   وب سرویس تبدیل کدهای ترمینولوژی
        /// </summary>
        public static readonly string Url_convertByTerminology = "v1/convertByTerminology";

        /// <summary>
        /// وب سرویس چاپ نسخه ارائه شده
        /// </summary>
        public static readonly string Url_printPresentation = "v1/printPresentation";

        /// <summary>
        /// وب سرویس ثبت اولیه نسخه در نسخه پیچی کاغذی
        /// </summary>
        public static readonly string Url_registerInitialPaperPrescription = "v1/registerInitialPaperPrescription";

        /// <summary>
        /// وب سرویس ویرایش نسخه در نسخه پیچی
        /// </summary>
        public static readonly string Url_editPrescription = "v1/editPrescription";

        /// <summary>
        /// سرویس استحقاق سنجی
        /// </summary>
        public static readonly string Url_eligibleHid = "v1/eligibleHid";

        /// <summary>
        /// سرویس واکشی لیست اطلاعات خدمات یک ترمینولوژی
        /// </summary>
        public static readonly string Url_getAllServiceListByType = "v2/getAllServiceListByType";

        /// <summary>
        /// سرویس واکشی لیست اطلاعات خدمات یک ترمینولوژی
        /// </summary>
        public static readonly string Url_getServiceByTypeAndCode = "v2/getServiceByTypeAndCode";

        /// <summary>
        /// سرویس واکشی لیست نسخ نیاز به بارگذاری مدارک
        /// </summary>
        public static readonly string Url_getPrescriptionHaveDocumentFile = "v2/getPrescriptionHaveDocumentFile";

        /// <summary>
        /// سرویس بارگذاری مدارک نسخه/خدمت
        /// </summary>
        public static readonly string Url_uploadMultiple = "v2/uploadMultiple";

        /// <summary>
        /// سرویس حذف مدارک بارگذاری شده نسخه/خدمت
        /// </summary>
        public static readonly string Url_deleteUploadedDocumentFile = "v2/deleteUploadedDocumentFile";

        /// <summary>
        /// سرویس فعال سازی بارکد در نسخه پیچی  الکترونیک / کاغذی
        /// </summary>
        public static readonly string Url_deleteAuthenticity = "v1/deleteAuthenticity";// ساخد راهنماش اذیتم کرد

        /// <summary>
        /// وب سرویس سرویس لیست تجویزکنندگان مرکز
        /// </summary>
        public static readonly string Url_centerDoctorList = "v1/centerDoctorList";

        /// <summary>
        /// وب سرویس پذیرش (نوبت دهی) در نسخه نویسی
        /// </summary>
        public static readonly string Url_getAppointment = "v1/getAppointment";

        /// <summary>
        /// وب سرویس لیست مراجعین پذیرش در نسخه نویسی
        /// </summary>
        public static readonly string Url_getClientListTajviz = "v1/getClientListTajviz";

        /// <summary>
        /// وب سرویس حذف مراجعین پذیرش در نسخه نویسی
        /// </summary>
        public static readonly string Url_deleteAppointment = "v1/deleteAppointment";

        /// <summary>
        /// سرویس ثبت نسخه تجویز الکترونیک (شامل ثبت، اضافه نمودن و حذف قلم)
        /// </summary>
        public static readonly string Url_registerPrescription = "v1/registerPrescription";

        /// <summary>
        /// وب سرویس واکشی اقلام تجویزی توسط تجویزکننده با یک کد رهگیری
        /// </summary>
        public static readonly string Url_fetchPrescription = "v1/fetchPrescription";

        /// <summary>
        /// وب سرویس چاپ نسخه تجویزی
        /// </summary>
        public static readonly string Url_printPrescribe = "v1/printPrescribe";

        /// <summary>
        /// وب سرویس تاریخچه تجویز
        /// </summary>
        public static readonly string Url_getHistory = "v1/getHistory";

        /// <summary>
        /// وب سرویس واکشی جزئیات (اقلام) تاریخچه تجویز
        /// </summary>
        public static readonly string Url_getSubHistory = "v1/getSubHistory";

        /// <summary>
        /// سرویس واکشی لیست خدمات خارج از نسخه
        /// </summary>
        public static readonly string Url_getAllUnPrescribedServiceListByType = "v2/getAllUnPrescribedServiceListByType";

        /// <summary>
        /// سرویس استعلام اولیه تجویز نسخه الکترونیک
        /// </summary>
        public static readonly string Url_inquiryPrescription = "v1/inquiryPrescription";

        /// <summary>
        /// سرویس واکشی لیست مسئولین فنی مرکز جاری
        /// </summary>
        public static readonly string Url_TechnicalManagerList = "v1/technicalManagerList";

        /// <summary>
        /// سرویس واکشی اطلاعات یک خدمت
        /// </summary>
        public static readonly string Url_GetServiceByTypeAndCode = "v2/getServiceByTypeAndCode";

        /// <summary>
        /// سرویس واکشی لیست انواع زمان مصرف دارو
        /// </summary>
        public static readonly string Url_getAllConsumptionListByShape = "v2/getAllConsumptionListByShape";

        /// <summary>
        /// سرویس واکشی لیست انواع میزان مصرف دارو
        /// </summary>
        public static readonly string Url_getAllConsumptionInstructionListByShape = "v2/getAllConsumptionInstructionListByShape";
    }

    public class Sakhad_StaticInfoWebServiceData
    {
        //SecureString secureString = new SecureString();
        //ProcessStartInfo startInfo = new ProcessStartInfo();

        /// <summary>
        /// فلگ اینکه نسخه اجرا شده تستس هست یا عملیاتی
        /// تستی  False
        /// عملیاتی True
        /// </summary>
        public static bool Ws_IsMainRun = false;

        /// <summary>
        /// لیست همه هدر های وب سرویس ها
        /// </summary>
        public static Dictionary<string, HttpRequestMessage> AllHeadersByURL1 = new Dictionary<string, HttpRequestMessage>();

        /// <summary>
        ///هدر فرمت ثابت ارسال و دریافت json
        /// </summary>
        public static string Ws_Content_Type = "application/json";

        /// <summary>
        ///به ازای هر شرکت نرم افزاری XIS مرکز درمانی مقدار clientId یونیک تعلق گرفته و از طریق سازمان بیمه )ساخد( در اختیار توسعه دهندگان قرار می گیرد.
        /// </summary>
        public static string Ws_clientId = "MedicalSciencesShiraz";

        /// <summary>
        ///به ازای هر ورژن شرکت نرم افزاری XIS مرکز درمانی مقدار clientSecret یونیک تعلق گرفته و از طریق سازمان بیمه )ساخد( در اختیار توسعه دهندگان قرار می گیرد.
        /// </summary>
        public static string Ws_clientSecret = "MedicalSciencesShirazSecret";

        /// <summary>
        ///شناسه منحصر به فرد کلاینت متصل شده به عامل-ترکیب ip و mac address
        /// </summary>
        public static string Ws_workstationid = "176.123.64.2";

        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public static int Ws_cid = 122091;

        /// <summary>
        /// نام کاربری پزشک یا منشی یا ارائه دهنده خدمت
        /// </summary>
        public static string Ws_username = "3874099024";

        /// <summary>
        /// رمز عبور ثابت
        /// </summary>
        public static string Ws_password = "Sb@bati@2025";

        /// <summary>
        /// شماره تلفن همراه کاربر مربوطه که در زمان ثبت نام به ساخد ارائه نموده است.
        /// </summary>
        public static string Ws_mobile = "09173838322";

        /// <summary>
        /// پیامک دریافتی از سرور برای رمز یکبار مصرف OTP
        /// </summary>
        public static string Ws_answer_SMS = "";

        /// <summary>
        /// 0== Sucsess
        /// </summary>
        public static int Ws_SucsessStatus = 0;

        public static CenterCodes_enum Ws_CenterCodes_enum;

        #region تغییر هدر ها

        /*
               private static string _Ws_requestId;
               private static string _Ws_expireAccessToken;
               private static string _Ws_sessionId;
               private static string _Ws_expireSessionId;
               private static string _Ws_accessToken;
               private static string _Ws_Authorization;

               /// <summary>
               /// شناسه درخواست که باید در هدر تمامی درخواست ها با همین نام ارسال شود
               /// </summary>
               public static string Ws_requestId
               {
                   get
                   {
                       return _Ws_requestId;
                   }
                   set
                   {
                       _Ws_requestId = value; // مقدار جدید را ذخیره کن
                       PopularStaticClass.CreateHeadersList(); // تابع تغییر هدر ها رو صدا بزن
                   }
               }

               /// <summary>
               /// زمان انقضای توکن است
               /// </summary>
               public static string Ws_expireAccessToken
               {
                   get
                   {
                       return _Ws_expireAccessToken;
                   }
                   set
                   {
                       _Ws_expireAccessToken = value; // مقدار جدید را ذخیره کن
                       PopularStaticClass.CreateHeadersList(); // تابع تغییر هدر ها رو صدا بزن
                   }
               }

               /// <summary>
               /// نشست کاربر
               /// </summary>
               public static string Ws_sessionId
               {
                   get
                   {
                       return _Ws_sessionId;
                   }
                   set
                   {
                       _Ws_sessionId = value; // مقدار جدید را ذخیره کن
                       PopularStaticClass.CreateHeadersList(); // تابع تغییر هدر ها رو صدا بزن
                   }
               }

               /// <summary>
               /// زمان انقضای نشست کاربر است
               /// </summary>
               public static string Ws_expireSessionId
               {
                   get
                   {
                       return _Ws_expireSessionId;
                   }
                   set
                   {
                       _Ws_expireSessionId = value; // مقدار جدید را ذخیره کن
                       PopularStaticClass.CreateHeadersList(); // تابع تغییر هدر ها رو صدا بزن
                   }
               }

               /// <summary>
               ///توکن دریافت شده در سرویس ورود مرحله 2 به صورت Bearer {token}
               ///نکته باحال همون اسم دیگه  accessToken
               /// </summary>
               public static string Ws_Authorization
               {
                   get
                   {
                       return "Bearer " + _Ws_Authorization;
                   }
                   set
                   {
                       _Ws_Authorization = value; // مقدار جدید را ذخیره کن
                       PopularStaticClass.CreateHeadersList(); // تابع تغییر هدر ها رو صدا بزن
                   }
               }

               /// <summary>
               /// توکن دسترسی به سرویس ها
               /// </summary>
               public static string Ws_accessToken
               {
                   get
                   {
                       return _Ws_accessToken;
                   }
                   set
                   {
                       _Ws_accessToken = value; // مقدار جدید را ذخیره کن
                       _Ws_Authorization = value;
                       PopularStaticClass.CreateHeadersList(); // تابع تغییر هدر ها رو صدا بزن
                   }
               }
               */

        #endregion تغییر هدر ها
    }
}