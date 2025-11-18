using System.ComponentModel;

namespace WebApi_Sakhad_ZX
{
    public static class StaticInfo
    {
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
}