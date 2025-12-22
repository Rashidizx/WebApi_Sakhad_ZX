using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using WebApi_Sakhad_ZX.Models;

namespace WebApi_Sakhad_ZX
{
    public static class WebServiceCaller
    {
        // Single HttpClient instance for whole AppDomain (recommended pattern)
        private static readonly HttpClient _httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(2) };

        /// <summary>
        /// فراخوانی وب‌سرویس به صورت جنریک با مدیریت هوشمند خطا و بازگشت مقدار پیش‌فرض بر اساس نوع خروجی
        /// </summary>
        public static async Task<TResponse> CallAsync<TRequest, TResponse>(
          SakhadCenter SelectedCenter,
            string miniUrl,
            TRequest requestObj,
            TResponse defaultErrorResponse = null
        ) where TResponse : class, new()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(miniUrl))
                    return CreateDefaultError<TResponse>("آدرس خالیه برای وب سرویس");

                if (requestObj == null)
                    return CreateDefaultError<TResponse>("آبجکت درخواست نال هست");

                var fullUrl = Sakhad_StaticInfoURL.GetFullUrl(miniUrl);
                if (miniUrl == Sakhad_StaticInfoURL.Url_verifyCaptcha)
                    fullUrl = Sakhad_StaticInfoURL.GetFullUrl(miniUrl, false);



                if (!SelectedCenter.AllHeadersByURL.TryGetValue(miniUrl, out var wsHeaderData))
                    return CreateDefaultError<TResponse>("هدر های داینامیک برای افزودن پیدا نشد");
                
               


                string jsonRequest = PopularStaticClass.ConvertClassToJson(requestObj);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");


                var headers = wsHeaderData.Headers;

                _httpClient.DefaultRequestHeaders.Clear();
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders. TryAddWithoutValidation(header.Key, header.Value);
                }



                try
                {
                    HttpResponseMessage httpResponse = null;

                    httpResponse = await _httpClient.PostAsync(fullUrl, content).ConfigureAwait(false);

                    var jsonResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        // اینجا گفتم سعی کن تبدیل کنی به نوع کلاس ورودی و اگه نشد تبدیل کن کلاس رفع محدودیت
                        var result = JsonHelper.ConvertJsonWithFallback<TResponse, UnlockByCaptchaVerificationResponse>(jsonResponse);

                        // اینجا هم گفتم اگه نتونستی تبدیل کنی به بالایی ها تبدیل کن کلاس خطا
                        if (result == null)
                            result = CreateDefaultError<TResponse>("پاسخ دریافتی سازگار نیست.");
                        else
                        {
                            return result = PopularStaticClass.ConvertJsonToClass<TResponse>(jsonResponse);
                        }

                        //var result = PopularStaticClass.ConvertJsonToClass<TResponse>(jsonResponse);// JsonConvert.DeserializeObject<TResponse>(jsonResponse);
                        //return result ?? CreateDefaultError<TResponse>("پاسخ دریافتی سازگار نیست.");
                        // if ( )
                    }
                    else
                    {
                        if (jsonResponse.Contains(" حد مجاز می باشد"))
                        {
                            var _result = PopularStaticClass.ConvertJsonToClass<UnlockByCaptchaVerificationResponse>(jsonResponse);
                            //var v = new FrmVerifyCaptcha(_result.data.FirstOrDefault().captcha);
                            //v.BringToFront();
                            //v.TopLevel = true;
                            //v.TopMost = true;
                            //v.ShowDialog();
                        }
                    }

                    return CreateDefaultError<TResponse>($"Server Error: {jsonResponse}");
                }
                catch (Exception zx)
                {
                    zx.Log();
                    // اگر Wrapper اختصاصی ست، مقدار defaultErrorResponse را برمی‌گرداند، وگرنه یک مدل هوشمند می‌سازد
                    if (defaultErrorResponse != null)
                        return defaultErrorResponse;
                    return CreateDefaultError<TResponse>(zx.Message);
                }
            }
            catch (Exception zx)
            {
                zx.Log();
                // اگر Wrapper اختصاصی ست، مقدار defaultErrorResponse را برمی‌گرداند، وگرنه یک مدل هوشمند می‌سازد
                if (defaultErrorResponse != null)
                    return defaultErrorResponse;
                return CreateDefaultError<TResponse>(zx.Message);
            }
        }

        /// <summary>
        /// تولید خروجی خطای پیش‌فرض بسته به پراپرتی‌های موجود در مدل خروجی
        /// </summary>
        private static T CreateDefaultError<T>(string message) where T : class, new()
        {
            var t = new T();
            var type = typeof(T);

            // پراپرتی‌های رایج: status، Flag، Message
            var statusProp = type.GetProperty("status", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var flagProp = type.GetProperty("flag", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var messageProp = type.GetProperty("message", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            statusProp?.SetValue(t, 4);
            flagProp?.SetValue(t, false);
            messageProp?.SetValue(t, message);

            // الگوی Generic Error Contract (IsSuccess/Message)
            var isSuccessProp = type.GetProperty("IsSuccess", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            isSuccessProp?.SetValue(t, false);

            return t;
        }
    }
}