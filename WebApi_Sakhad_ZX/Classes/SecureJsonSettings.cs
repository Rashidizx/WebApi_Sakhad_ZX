using Newtonsoft.Json;

namespace WebApi_Sakhad_ZX.Classes
{
    internal static class SecureJsonSettings
    {
        public static readonly JsonSerializerSettings Default = new JsonSerializerSettings
        {
            //   جلوگیری از حملات Polymorphic (خیلی مهم)
            TypeNameHandling = TypeNameHandling.None,

            //   نال‌ها اذیت نکنند
            NullValueHandling = NullValueHandling.Ignore,

            //   اگر فیلدی در JSON نبود، برنامه نریزد
            MissingMemberHandling = MissingMemberHandling.Ignore,

            //   کنترل خطا بدون Exception Bomb
            Error = (sender, args) =>
            {
                args.ErrorContext.Handled = true;
            }
        };
    }
}