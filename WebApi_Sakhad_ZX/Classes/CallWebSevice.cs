using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX
{
    public class CallWebSevice
    {  /// <summary>
       /// متد ارسال رمز ثابت
       /// </summary>
       /// <param name="_LoginRequest"></param>
       /// <returns> یه کلاس از LoginResponse </returns>
        public static async Task<LoginResponse> FnLoginAsync(LoginRequest _LoginRequest, SakhadCenter SelectedCenter)//2-3
        {
            return await WebServiceCaller.CallAsync<LoginRequest, LoginResponse>(SelectedCenter, Sakhad_StaticInfoURL.Url_login, _LoginRequest);
        }

        /// <summary>
        /// وب سرویس تایید رمز ثابت
        /// </summary>
        /// <param name="_ConfirmRequest"></param>
        /// <returns></returns>
        public static async Task<ConfirmResponse> FnConfirmAsync(ConfirmRequest _ConfirmRequest, SakhadCenter SelectedCenter)//2-4
        {
            return await WebServiceCaller.CallAsync<ConfirmRequest, ConfirmResponse>(SelectedCenter, Sakhad_StaticInfoURL.Url_confirm, _ConfirmRequest);
        }

        /// <summary>
        /// وب سرویس درخواست مجدد کپچا برای تایید رمز ثابت
        /// </summary>
        /// <param name="_GetCaptchaRequest"></param>
        /// <returns></returns>
        public static async Task<GetCaptchaResponse> FnGetCaptchaAsync(GetCaptchaRequest _GetCaptchaRequest, SakhadCenter SelectedCenter)//2-5
        {
            return await WebServiceCaller.CallAsync<GetCaptchaRequest, GetCaptchaResponse>(SelectedCenter, Sakhad_StaticInfoURL.Url_getCaptcha, _GetCaptchaRequest);
        }

        /// <summary>
        /// وب سرویس پاسخ به کپچا برای رفع محدودیت دسترسی به سرویس ها
        /// </summary>
        /// <param name="_verifyCaptchaRequest"></param>
        /// <returns></returns>
        public static async Task<verifyCaptchaResponse> FnVerifyCaptchaAsync(verifyCaptchaRequest _verifyCaptchaRequest, SakhadCenter SelectedCenter)//2-7
        {
            return await WebServiceCaller.CallAsync<verifyCaptchaRequest, verifyCaptchaResponse>(SelectedCenter, Sakhad_StaticInfoURL.Url_verifyCaptcha, _verifyCaptchaRequest);
        }

        /// <summary>
        /// وب سرویس استعلام اطلاعات هویتی و بیمه ای
        /// </summary>
        /// <param name="_EligibleRequest"></param>
        /// <returns></returns>
        public static async Task<EligibleResponse> FnEligibleAsync(EligibleRequest _EligibleRequest, SakhadCenter SelectedCenter)//3-1
        {
            return await WebServiceCaller.CallAsync<EligibleRequest, EligibleResponse>(SelectedCenter, Sakhad_StaticInfoURL.Url_eligible, _EligibleRequest);
        }

        /// <summary>
        /// وب سرویس واکشی اقلام تجویزی پزشک در نسخه پیچی الکترونیک
        /// </summary>
        /// <param name="_getPrescribeItemsListRequest"></param>
        /// <returns></returns>
        public static async Task<getPrescribeItemsListResponse> FnGetPrescribeItemsListAsync(getPrescribeItemsListRequest _getPrescribeItemsListRequest, SakhadCenter SelectedCenter)//3-17
        {
            return await WebServiceCaller.CallAsync<getPrescribeItemsListRequest, getPrescribeItemsListResponse>(SelectedCenter, Sakhad_StaticInfoURL.Url_getPrescribeItemsList, _getPrescribeItemsListRequest);
        }
    }
}