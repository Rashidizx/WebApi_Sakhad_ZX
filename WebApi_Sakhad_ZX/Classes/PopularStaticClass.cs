using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

namespace WebApi_Sakhad_ZX
{
    internal static class PopularStaticClass
    {
        /// <summary>
        /// روی پراپرتی  ها و همه اعمال میشه
        /// </summary>
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.All)] // فقط روی پراپرتی‌ها قابل اعمال است
        public class PersianNameAttribute : Attribute
        {
            public string Name { get; }

            public PersianNameAttribute(string name) => Name = name;
        }

        /// <summary>
        /// یه کلاس که فقط دو پراپرتی داره
        /// </summary>
        public class Bool_string
        {
            public Boolean flag { get; set; }
            public string WarningMsg { get; set; }
        }

        /// <summary>
        /// اعتبار سنجی کد ملی
        /// </summary>
        /// <param name="nationalCode"></param>
        /// <returns>یه کلاس برمیگردونه که صحیح یا غلط بودن رو میده و پیام خطا احتمالی</returns>
        public static Bool_string IsValidNationalCode(this String nationalCode)
        {
            try
            {
                //در صورتی که کد ملی وارد شده تهی باشد

                if (String.IsNullOrEmpty(nationalCode))
                {
                    return new Bool_string() { flag = false, WarningMsg = "لطفا کد ملی را صحیح وارد نمایید" };
                }

                //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
                if (nationalCode.Length != 10)
                    return new Bool_string() { flag = false, WarningMsg = "طول کد ملی باید ده رقم باشد" };

                //در صورتی که کد ملی ده رقم عددی نباشد
                var regex = new Regex(@"\d{10}");
                if (!regex.IsMatch(nationalCode))
                    return new Bool_string() { flag = false, WarningMsg = "کد ملی تشکیل شده از ده رقم عددی می‌باشد؛ لطفا کد ملی را صحیح وارد نمایید" };

                //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
                var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
                if (allDigitEqual.Contains(nationalCode)) return new Bool_string() { flag = false, WarningMsg = "نمیتواند یکسان باشد" };

                //عملیات شرح داده شده در بالا
                var chArray = nationalCode.ToCharArray();
                var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
                var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
                var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
                var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
                var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
                var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
                var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
                var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
                var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
                var a = Convert.ToInt32(chArray[9].ToString());

                var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
                var c = b % 11;

                return new Bool_string()
                { flag = (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a))), WarningMsg = "کد ملی صحیح نیست" };
            }
            catch (Exception zx)
            {
                zx.Log();
                return new Bool_string() { flag = false, WarningMsg = "نمیتواند یکسان باشد" };
            }
        }

     
       
        /// <summary>
        /// یه نوع مپینگ
        /// </summary>
        /// <param name="ParameterValue"></param>
        /// <param name="ParameterName"></param>
        /// <returns></returns>
        private static string Fn_ifNeed2Map(object ParameterValue, string ParameterName)
        {
            ParameterName = ParameterName.ToLower().Trim();
            switch (ParameterName.ToLower())
            {
                case "gender":
                    return MapBoolInt(ParameterValue, "مرد", "زن");

                case "emergency":
                    return MapBoolInt(ParameterValue, "انجام اورژانسی", "-");

                case "otc":
                case "shouldsendtoesalat":
                    return MapBoolInt(ParameterValue, "می باشد", "نمی باشد");

                case "iscovered":
                    return MapBoolInt(ParameterValue, "فعال", "غیرفعال");

                default:
                    return ParameterValue?.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        /// متد که هر نوع داده رو میگیره و تبدیل میکنه به دو نوع داده صحیح یا غلط
        /// </summary>
        /// <param name="val">متغییر ورودی</param>
        /// <param name="trueVal"> اگه مقدار صحیح بود اینو بده</param>
        /// <param name="falseVal">اگه مقدار غلط بود اینو بده</param>
        /// <returns>رشته صحیح یا غلط </returns>
        private static string MapBoolInt(object val, string trueVal, string falseVal)
        {
            if (val is int i) return i == 1 ? trueVal : falseVal;
            if (val is bool b) return b ? trueVal : falseVal;
            if (val != null)
            {
                var s = val.ToString();
                if (s == "1" || s.ToLower() == "true") return trueVal;
                if (s == "0" || s.ToLower() == "false") return falseVal;
            }
            return val?.ToString() ?? string.Empty;
        }

   
        ///// <summary>
        ///// تبدیل رشته به تصویر
        ///// </summary>
        ///// <param name="base64String">base64String رشته برای کپچا یا عکس</param>
        ///// <returns></returns>
        //public static  Image ConvertBase64ToImage(string base64String)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(base64String)) return null;
        //        // Remove the data URI prefix if present
        //        if (base64String.Contains(","))
        //        {
        //            base64String = base64String.Split(',')[1];
        //        }

        //        // Convert base64 string to byte array
        //        byte[] imageBytes = Convert.FromBase64String(base64String);

        //        // Create memory stream from byte array
        //        using (var ms = new MemoryStream(imageBytes))
        //        {
        //            // Create image from stream
        //            Image image = Image.FromStream(ms);
        //            return image;
        //        }
        //    }
        //    catch (Exception zx)
        //    {
        //        zx.Log();
        //        Console.WriteLine($"Error converting Base64 to Image: {zx.Message}");
        //        return null;
        //    }
        //}

 
        /// <summary>
        /// تبدیل تاریخ به شمسی جاری سیستم
        /// </summary>
        /// <returns>ثلا تاریخ 1403/11/03 رو به صورت 14031103"</returns>
        public static string ConvertPersianDateToCompact()
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime now = DateTime.Now;

            string year = pc.GetYear(now).ToString();
            string month = pc.GetMonth(now).ToString("00");
            string day = pc.GetDayOfMonth(now).ToString("00");

            return $"{year}{month}{day}";
        }

        private static void SetIfNotEmpty(Action<string> setter, string value)
        {
            if (!string.IsNullOrEmpty(value))
                setter(value);
        }

        /// <summary>
        /// افزودن مقدار جدید در هدر ها
        /// </summary>
        /// <param name="_sessionId">نشست کاربر</param>
        /// <param name="_requestId">شناسه درخواست که باید در هدر تمامی درخواست ها با همین نام ارسال شود</param>
        /// <param name="_expireSessionId">زمان انقضای نشست کاربر است  nullAble</param>
        public static void FnSetHeaders(string _sessionId, string _requestId, String _expireSessionId)
        {
            SetIfNotEmpty(v => Sakhad_StaticInfoWebServiceData.Ws_sessionId = v, _sessionId);
            SetIfNotEmpty(v => Sakhad_StaticInfoWebServiceData.Ws_requestId = v, _requestId);
            SetIfNotEmpty(v => Sakhad_StaticInfoWebServiceData.Ws_expireSessionId = v, _expireSessionId);
        }

        /// <summary>
        ///  افزودن مقدار جدید در هدر ها
        /// </summary>
        /// <param name="_expireSessionId">زمان انقضای نشست کاربر است  nullAble</param>
        public static void FnSetHeaders(string _expireSessionId)
        {
            SetIfNotEmpty(v => Sakhad_StaticInfoWebServiceData.Ws_expireSessionId = v, _expireSessionId);
        }

        /// <summary>
        /// افزودن مقدار جدید در هدر ها
        /// </summary>
        /// <param name="_accessToken">توکن دسترسی به سرویس ها</param>
        /// <param name="_expireAccessToken">زمان انقضای توکن است</param>
        public static void FnSetHeaders(string _accessToken, string _expireAccessToken)
        {
            SetIfNotEmpty(v => Sakhad_StaticInfoWebServiceData.Ws_Authorization = v, _accessToken);
            SetIfNotEmpty(v => Sakhad_StaticInfoWebServiceData.Ws_accessToken = v, _accessToken);
            SetIfNotEmpty(v => Sakhad_StaticInfoWebServiceData.Ws_expireAccessToken = v, _expireAccessToken);
        }

        /// <summary>
        /// ایجاد هدر برای هر لینک وب سرویس جدا
        /// </summary>
        public static void CreateHeadersList()
        {
            try
            {
                Dictionary<string, HttpRequestMessage> HeadersByURL = new Dictionary<string, HttpRequestMessage>();

                var HttpRequest_Login = new HttpRequestMessage();
                //HttpRequest_Login.Headers.Add("Content-Type", Sakhad_StaticInfoWebServiceData.Ws_Content_Type);
                HttpRequest_Login.Headers.Add("clientId", Sakhad_StaticInfoWebServiceData.Ws_clientId);
                HttpRequest_Login.Headers.Add("clientSecret", Sakhad_StaticInfoWebServiceData.Ws_clientSecret);
                HttpRequest_Login.Headers.Add("workstationid", Sakhad_StaticInfoWebServiceData.Ws_workstationid);

                var HttpRequest_getCaptcha = new HttpRequestMessage();
                //HttpRequest_Login.Headers.Add("Content-Type", Sakhad_StaticInfoWebServiceData.Ws_Content_Type);
                HttpRequest_getCaptcha.Headers.Add("clientId", Sakhad_StaticInfoWebServiceData.Ws_clientId);
                HttpRequest_getCaptcha.Headers.Add("clientSecret", Sakhad_StaticInfoWebServiceData.Ws_clientSecret);
                HttpRequest_getCaptcha.Headers.Add("workstationid", Sakhad_StaticInfoWebServiceData.Ws_workstationid);
                HttpRequest_getCaptcha.Headers.Add("requestid", Sakhad_StaticInfoWebServiceData.Ws_requestId);

                var HttpRequest_MostWorking = new HttpRequestMessage();
                //  HttpRequest_MostWorking.Headers.Add("Content-Type", Sakhad_StaticInfoWebServiceData.Ws_Content_Type);
                HttpRequest_MostWorking.Headers.Add("Authorization", Sakhad_StaticInfoWebServiceData.Ws_Authorization);
                HttpRequest_MostWorking.Headers.Add("sessionId", Sakhad_StaticInfoWebServiceData.Ws_sessionId);
                HttpRequest_MostWorking.Headers.Add("requestId", Sakhad_StaticInfoWebServiceData.Ws_requestId);

                var HttpRequest_verify_otp = new HttpRequestMessage();
                //  HttpRequest_verify_otp.Headers.Add("Content-Type", Sakhad_StaticInfoWebServiceData.Ws_Content_Type);
                HttpRequest_verify_otp.Headers.Add("requestId", Sakhad_StaticInfoWebServiceData.Ws_requestId);

                var HttpRequest_verifyCaptcha = new HttpRequestMessage();
                //HttpRequest_verifyCaptcha.Headers.Add("Content-Type", Sakhad_StaticInfoWebServiceData.Ws_Content_Type);
                HttpRequest_verifyCaptcha.Headers.Add("Authorization", Sakhad_StaticInfoWebServiceData.Ws_Authorization);

                var HttpRequest_confirm = new HttpRequestMessage();
                //    HttpRequest_confirm.Headers.Add("Content-Type", Sakhad_StaticInfoWebServiceData.Ws_Content_Type);
                HttpRequest_confirm.Headers.Add("clientId", Sakhad_StaticInfoWebServiceData.Ws_clientId);
                HttpRequest_confirm.Headers.Add("clientSecret", Sakhad_StaticInfoWebServiceData.Ws_clientSecret);
                HttpRequest_confirm.Headers.Add("workstationid", Sakhad_StaticInfoWebServiceData.Ws_workstationid);
                HttpRequest_confirm.Headers.Add("requestId", Sakhad_StaticInfoWebServiceData.Ws_requestId);

                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_verify_otp, HttpRequest_verify_otp);

                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_verifyCaptcha, HttpRequest_verifyCaptcha);

                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_confirm, HttpRequest_confirm);

                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_send_otp, HttpRequest_Login);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_login, HttpRequest_Login);

                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getCaptcha, HttpRequest_getCaptcha);

                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getToken, HttpRequest_Login);

                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_eligible, HttpRequest_MostWorking);

                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_TechnicalManagerList, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getSpecialityListByName, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_inquiryDoctor, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getPrescribeItemsList, HttpRequest_MostWorking);

                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getAllConsumptionListByShape, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getAllConsumptionInstructionListByShape, HttpRequest_MostWorking);

                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getAllUnPrescribedServiceListByType, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_registerInitialPrescription, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_registerInitialPaperPrescription, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_confirmPrescriptionPresentation, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_deletePrescription, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_inquiryAuthenticity, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_deleteAuthenticity, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getHistoryPresentation, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getSubHistoryPresentation, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_convertByTerminology, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_printPresentation, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_printPrescribe, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_editPrescription, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_eligibleHid, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getAllServiceListByType, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getServiceByTypeAndCode, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_getPrescriptionHaveDocumentFile, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_uploadMultiple, HttpRequest_MostWorking);
                HeadersByURL.Add(Sakhad_StaticInfoURL.Url_deleteUploadedDocumentFile, HttpRequest_MostWorking);

                Sakhad_StaticInfoWebServiceData.AllHeadersByURL = HeadersByURL;
            }
            catch (Exception zx)
            {
                zx.Log();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TRequest">نوع کلاس برای مپ کردن به جیسون</typeparam>
        /// <param name="requestObj"> اینستنس از کلاس برای مپ کردن</param>
        /// <returns>خروجی جیسون</returns>
        internal static string ConvertClassToJson<TRequest>(TRequest requestObj)
        {
            return JsonConvert.SerializeObject(requestObj);
            return JsonConvert.SerializeObject(requestObj, Newtonsoft.Json.Formatting.None,
    new JsonSerializerSettings
    {
        ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            SerializeCompilerGeneratedMembers = true
        }
    });
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResponse">نوع کلاس برای مپ کردن از جیسون</typeparam>
        /// <param name="jsonResponse">رشته جیسون برای تبدیل به اینستس کلاس</param>
        /// <returns>یه اینستنس از کلاس مپ شده</returns>
        internal static TResponse ConvertJsonToClass<TResponse>(string jsonResponse)
        {
#warning گهکاهی مقادیر نال اذیت میکنه
            //گهکاهی مقادیر نال اذیت میکنه fix me
            return JsonConvert.DeserializeObject<TResponse>(jsonResponse);
        }

        /// <summary>
        /// مقادیر پراپرتی‌های مشخص‌شده را از یک شیء برمی‌گرداند.
        /// اگر پراپرتی وجود نداشت یا مقدارش null بود، مقدار پیش‌فرض همان تایپ برگردانده می‌شود.
        /// </summary>
        /// <typeparam name="T">نوع مقدار خروجی</typeparam>
        /// <param name="obj">اینستنس کلاسی که می‌خواهیم از آن پراپرتی‌ها را بخوانیم</param>
        /// <param name="propertyName">نام پراپرتی</param>
        /// <returns>مقدار پراپرتی یا مقدار پیش‌فرض</returns>
        public static T GetPropertyValueSafe<T>(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrWhiteSpace(propertyName))
                return default;

            var prop = obj.GetType().GetProperty(propertyName);
            if (prop == null)
            {
                // اگر نوع خروجی عددی است: 4
                if (typeof(T) == typeof(int))//وقتیstatus مقدار نداشت یه مقدار غیر صفر بده
                    return (T)Convert.ChangeType(4, typeof(T));
                return default;
            }

            var value = prop.GetValue(obj);
            if (value == null)
                return default;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default;
            }
        }

      

    
      
        /// <summary>
        /// مقایسه الگو در متن
        /// </summary>
        /// <param name="str_raw">متنن خام ورودی</param>
        /// <param name="searchText">الگوی متنی که باید پیدا بشه</param>
        /// <returns>خروجی میگه پیدا شد یا نشد</returns>
        public static bool PatternContains(string str_raw, string searchText)
        {
            if (string.IsNullOrWhiteSpace(str_raw) || string.IsNullOrWhiteSpace(searchText))
                return false;

            //str_raw = str_raw.Trim().ToLower();
            //searchText = searchText.Trim().ToLower();

            return str_raw.Contains(searchText);
        }

    


       

       
        public static string FnToPersianDate(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(date);
            int month = pc.GetMonth(date);
            int day = pc.GetDayOfMonth(date);

            // قالب خروجی: yyyy/MM/dd
            return string.Format("{0:0000}{1:00}{2:00}", year, month, day);
        }

        public static string FnToPersianDate_startMounts(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(date);
            int month = pc.GetMonth(date);
            int day = pc.GetDayOfMonth(date);

            // قالب خروجی: yyyy/MM/dd
            return string.Format("{0:0000}/{1:00}/01", year, month);
        }

        internal static List<string> FnGetFileName(List<string> fileNameList)
        {
            List<string> temp = new List<string>();
            foreach (var item in fileNameList)
                temp.Add(Path.GetFileName(item));

            return temp;
        }

       
    }
}