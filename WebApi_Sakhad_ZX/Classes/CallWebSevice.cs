using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX
{
    public class CallWebSevice
    {  /// <summary>
        /// متد ارسال رمز ثابت
        /// </summary>
        /// <param name="_LoginRequest"></param>
        /// <returns> یه کلاس از LoginResponse </returns>
        public static async Task<LoginResponse> FnLoginAsync(LoginRequest _LoginRequest)//2-3
        {
            return await WebServiceCaller.CallAsync<LoginRequest, LoginResponse>(Sakhad_StaticInfoURL.Url_login, _LoginRequest);
        }

        /// <summary>
        /// وب سرویس تایید رمز ثابت
        /// </summary>
        /// <param name="_ConfirmRequest"></param>
        /// <returns></returns>
        public static async Task<ConfirmResponse> FnConfirmAsync(ConfirmRequest _ConfirmRequest)//2-4
        {
            return await WebServiceCaller.CallAsync<ConfirmRequest, ConfirmResponse>(Sakhad_StaticInfoURL.Url_confirm, _ConfirmRequest);
        }

        /// <summary>
        /// وب سرویس درخواست مجدد کپچا برای تایید رمز ثابت
        /// </summary>
        /// <param name="_GetCaptchaRequest"></param>
        /// <returns></returns>
        public static async Task<GetCaptchaResponse> FnGetCaptchaAsync(GetCaptchaRequest _GetCaptchaRequest)//2-5
        {
            return await WebServiceCaller.CallAsync<GetCaptchaRequest, GetCaptchaResponse>(Sakhad_StaticInfoURL.Url_getCaptcha, _GetCaptchaRequest);
        }


        /// <summary>
        /// وب سرویس پاسخ به کپچا برای رفع محدودیت دسترسی به سرویس ها
        /// </summary>
        /// <param name="_verifyCaptchaRequest"></param>
        /// <returns></returns>
        public static async Task<verifyCaptchaResponse> FnVerifyCaptchaAsync(verifyCaptchaRequest _verifyCaptchaRequest)//2-7
        {
            return await WebServiceCaller.CallAsync<verifyCaptchaRequest, verifyCaptchaResponse>(Sakhad_StaticInfoURL.Url_verifyCaptcha, _verifyCaptchaRequest);
        }

        /// <summary>
        /// وب سرویس استعلام اطلاعات هویتی و بیمه ای
        /// </summary>
        /// <param name="_EligibleRequest"></param>
        /// <returns></returns>
        public static async Task<EligibleResponse> FnEligibleAsync(EligibleRequest _EligibleRequest)//3-1
        {
            return await WebServiceCaller.CallAsync<EligibleRequest, EligibleResponse>(Sakhad_StaticInfoURL.Url_eligible, _EligibleRequest);
        }

        /// <summary>
        /// وب سرویس واکشی اقلام تجویزی پزشک در نسخه پیچی الکترونیک
        /// </summary>
        /// <param name="_getPrescribeItemsListRequest"></param>
        /// <returns></returns>
        public static async Task<getPrescribeItemsListResponse> FnGetPrescribeItemsListAsync(getPrescribeItemsListRequest _getPrescribeItemsListRequest)//3-17
        {
            return await WebServiceCaller.CallAsync<getPrescribeItemsListRequest, getPrescribeItemsListResponse>(Sakhad_StaticInfoURL.Url_getPrescribeItemsList, _getPrescribeItemsListRequest);
        }


    }
}