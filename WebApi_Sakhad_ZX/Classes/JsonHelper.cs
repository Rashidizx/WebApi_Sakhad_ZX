using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Reflection;
using WebApi_Sakhad_ZX.Classes;

namespace WebApi_Sakhad_ZX
{
    internal class JsonHelper
    {
        /// <summary>
        /// تلاش می‌کند JSON را ابتدا به نوع اصلی تبدیل کند، و در صورت عدم موفقیت به نوع جایگزین.
        /// </summary>
        public static TPrimary ConvertJsonWithFallback<TPrimary, TFallback>(string json)
            where TPrimary : class, new()
            where TFallback : class
        {
            if (string.IsNullOrWhiteSpace(json))
                return new TPrimary();

            try
            {
                // تست مستقیم به TPrimary
                var primaryObj = JsonConvert.DeserializeObject<TPrimary>(json, SecureJsonSettings.Default);
                if (primaryObj != null && HasSignificantData(primaryObj))
                    return primaryObj;
            }
            catch
            {
                // ادامه می‌دیم تا fallback رو تست کنیم
            }

            try
            {
                // اگر primary سازگار نبود، تست روی TFallback
                var fallbackObj = JsonConvert.DeserializeObject<TFallback>(json, SecureJsonSettings.Default);
                if (fallbackObj != null && HasSignificantData(fallbackObj))
                {
                    // می‌تونی اینجا mapping بزنی به TPrimary
                    return MapFallbackToPrimary<TPrimary, TFallback>(fallbackObj);
                }
            }
            catch
            {
                // هیچ‌کدوم نشد
            }

            return new TPrimary();
        }

        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propCache = new();

        // بررسی داشتن داده واقعی
        private static bool HasSignificantData(object obj)
        {
            var type = obj.GetType();
            var props = _propCache.GetOrAdd(type, t => t.GetProperties());

            foreach (var p in props)
            {
                if (p.GetValue(obj) != null)
                    return true;
            }
            return false;
        }

        // مپ ساده fallback → primary (در صورت داشتن پروپرتی‌های مشابه)
        private static TPrimary MapFallbackToPrimary<TPrimary, TFallback>(TFallback fallbackObj)
            where TPrimary : class, new()
        {
            var primary = new TPrimary();
            var primaryProps = typeof(TPrimary).GetProperties();
            var fallbackProps = typeof(TFallback).GetProperties();

            foreach (var p in primaryProps)
            {
                var sourceProp = Array.Find(fallbackProps, fp => fp.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));
                if (sourceProp != null)
                {
                    var value = sourceProp.GetValue(fallbackObj);
                    if (value != null && p.CanWrite)
                        p.SetValue(primary, value);
                }
            }
            return primary;
        }

        /// <summary>
        /// تبدیل کلاس به JSON و ذخیره در فایل (با زمان و مسیر سفارشی)
        /// </summary>
        /// <typeparam name="T">نوع داده‌ای که قراره ذخیره بشه</typeparam>
        /// <param name="obj">آبجکت مورد نظر</param>
        /// <param name="filePath">مسیر فایل. اگر خالی باشه، مسیر خودکار استفاده می‌شود.</param>
        /// <param name="prettyFormat">آیا JSON زیبا‌سازی شود؟</param>
        public static void SaveClassToJsonFile<T>(T obj, string filePath = "", bool prettyFormat = true)
        {
            try
            {
                if (obj == null)
                    throw new ArgumentNullException(nameof(obj), "ورودی کلاس نمی‌تواند null باشد.");

                // اگر مسیر خالی بود، فایل در پوشه برنامه ساخته می‌شود
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JsonLogs");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string stamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    filePath = Path.Combine(folder, $"data_{typeof(T).Name}_{stamp}.json");
                }

                // تنظیمات زیبا‌سازی یا فشرده‌سازی
                var formatting = prettyFormat ? Formatting.Indented : Formatting.None;

                // تبدیل به رشته JSON
                string json = JsonConvert.SerializeObject(obj, formatting);

                // نوشتن در فایل
                File.WriteAllText(filePath, json);

                Console.WriteLine($"✅ فایل JSON با موفقیت ذخیره شد در مسیر: {filePath}");
            }
            catch (Exception zx)
            {
                zx.Log();
                Console.WriteLine($"❌ خطا در ذخیره فایل JSON: {zx.Message}");
            }
        }

        /// <summary>
        /// خواندن کلاس از فایل JSON (Deserialization)
        /// </summary>
        public static T LoadClassFromJsonFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("فایل مورد نظر پیدا نشد.", filePath);

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json, SecureJsonSettings.Default);
        }
    }
}