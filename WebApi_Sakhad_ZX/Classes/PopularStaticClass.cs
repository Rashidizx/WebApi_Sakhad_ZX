using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

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
        /// یه متد خوشگل  که بهش کلاس پاس میدی و پراپرتی های دارای صفت خاص رو برمیگردونه و تبدیل میکنه یه لیبل
        /// </summary>
        /// <param name="items">ارایه یا لیست کلاس ها</param>
        /// <param name="panel">کنترل پاس د اده شده برای افزودن کنترل یا کنترل ها</param>
        public static void CreateAndAddPersianLabels(object instance, FlowLayoutPanel panel)
        {
            if (instance == null || panel == null) return;

            panel.SuspendLayout();
            panel.Controls.Clear();

            // تعریف لیست اشیاء
            IEnumerable items;
            if (instance is IEnumerable enumerable && !(instance is string))
                items = enumerable;
            else
                items = new[] { instance };

            foreach (var obj in items)
            {
                if (obj == null) continue;
                var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in props)
                {
                    if (prop.GetCustomAttribute<PersianNameAttribute>() is PersianNameAttribute attr)
                    {
                        var val = prop.GetValue(obj)?.ToString() ?? "";
                        var edtNCode = new ShafaCommon.UIControls.EditView.EditTextBox
                        {
                            Caption = attr.Name + ": ",
                            CaptionHeight = 14,
                            CaptionWidth = (attr.Name + ": ").Length * 8,
                            Name = prop.Name,
                            Value = Fn_ifNeed2Map(val, prop.Name),
                            ValueHeight = 21,
                            ValueWidth = val.Length * 8,
                            FontLable = new System.Drawing.Font("Tahoma", 8.25F),
                            FontValue = new System.Drawing.Font("Tahoma", 8.25F, FontStyle.Bold),
                            lbValueColor = System.Drawing.SystemColors.WindowText,
                            RightToLeft = RightToLeft.Yes,
                            TextBoxRightToLeft = RightToLeft.Yes,
                            BackColor = System.Drawing.SystemColors.ControlLightLight,
                            Size = new Size(
                                (int)(((val.Length * 8) + ((attr.Name + ": ").Length * 8)) * 1.3),
                                21
                            ),
                            Margin = new Padding(5, 4, 5, 4)
                            //AllowDigit = ShafaCommon.Types.AllowState.Allow;
                            //Code = 0;
                            //FieldName = null;
                            //TextBoxBorder = System.Windows.Forms.BorderStyle.Fixed3D;
                            //CaptionAutoSizeEdit = false;
                            //CaptionAutoSizeView = false;
                            //AllowEnglish = false;
                            //AllowMinusChar = false;
                            //AllowPunctuation = false;
                            //IsCheckParentVisible = false;
                            //IsPassword = false;
                            //Location = new System.Drawing.Point(427, 163);
                            //MaxLength = 32767;
                            //MinLength = ((short)(2));
                            //Mode = ShafaCommon.Types.FormMode.View;
                            //MultiLine = false;
                            //edtNCode.TabIndex = 47;
                            // ترتیب چینش کنترل (در FlowLayoutPanel نیاز نیست، اما می‌گذاریم اگر خواستی تغییر بدی)
                        };

                        panel.Controls.Add(edtNCode);
                    }
                }
            }
            panel.ResumeLayout(true);
        }

        public static void FnClearAddedDynamicLables(FlowLayoutPanel panel)
        {
            try
            {
                foreach (Control item in panel.Controls.Cast<Control>().ToList())
                {
                    if (item is ShafaCommon.UIControls.EditView.EditTextBox edt)
                    {
                        panel.Controls.Remove(edt);
                        edt.Dispose();
                    }
                }
            }
            catch (Exception zx)
            {
                zx.Log();
            }
        }

        /// <summary>
        /// یه متد خوشگل  که بهش کلاس پاس میدی و پراپرتی های دارای صفت خاص رو برمیگردونه و تبدیل میکنه یه لیبل
        /// </summary>
        /// <param name="items">ارایه یا لیست کلاس ها</param>
        /// <param name="panel">کنترل پاس د اده شده برای افزودن کنترل یا کنترل ها</param>
        public static void CreateAndAddPersianLabels(object instance, FlowLayoutPanel panel, bool IsAppend)
        {
            try
            {
                if (instance == null || panel == null) return;

                panel.SuspendLayout();
                if (IsAppend)
                {
                    FnClearAddedDynamicLables(panel);
                }
                else
                {
                    panel.Controls.Clear();
                }
                // تعریف لیست اشیاء
                IEnumerable items;
                if (instance is IEnumerable enumerable && !(instance is string))
                    items = enumerable;
                else
                    items = new[] { instance };

                foreach (var obj in items)
                {
                    if (obj == null) continue;
                    var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var prop in props)
                    {
                        if (prop.GetCustomAttribute<PersianNameAttribute>() is PersianNameAttribute attr)
                        {
                            var val = prop.GetValue(obj)?.ToString() ?? "";
                            var edtNCode = new ShafaCommon.UIControls.EditView.EditTextBox
                            {
                                Caption = attr.Name + ": ",
                                CaptionHeight = 14,
                                CaptionWidth = (attr.Name + ": ").Length * 8,
                                Name = prop.Name,
                                Value = Fn_ifNeed2Map(val, prop.Name),
                                ValueHeight = 21,
                                ValueWidth = val.Length * 8,
                                FontLable = new System.Drawing.Font("Tahoma", 8.25F),
                                FontValue = new System.Drawing.Font("Tahoma", 8.25F, FontStyle.Bold),
                                lbValueColor = System.Drawing.SystemColors.WindowText,
                                RightToLeft = RightToLeft.Yes,
                                TextBoxRightToLeft = RightToLeft.Yes,
                                BackColor = System.Drawing.SystemColors.ControlLightLight,
                                Size = new Size(
                                    (int)(((val.Length * 8) + ((attr.Name + ": ").Length * 8)) * 1.3),
                                    21
                                ),
                                Margin = new Padding(5, 4, 5, 4)
                                //AllowDigit = ShafaCommon.Types.AllowState.Allow;
                                //Code = 0;
                                //FieldName = null;
                                //TextBoxBorder = System.Windows.Forms.BorderStyle.Fixed3D;
                                //CaptionAutoSizeEdit = false;
                                //CaptionAutoSizeView = false;
                                //AllowEnglish = false;
                                //AllowMinusChar = false;
                                //AllowPunctuation = false;
                                //IsCheckParentVisible = false;
                                //IsPassword = false;
                                //Location = new System.Drawing.Point(427, 163);
                                //MaxLength = 32767;
                                //MinLength = ((short)(2));
                                //Mode = ShafaCommon.Types.FormMode.View;
                                //MultiLine = false;
                                //edtNCode.TabIndex = 47;
                                // ترتیب چینش کنترل (در FlowLayoutPanel نیاز نیست، اما می‌گذاریم اگر خواستی تغییر بدی)
                            };

                            panel.Controls.Add(edtNCode);
                        }
                    }
                }
                panel.ResumeLayout(true);
            }
            catch (Exception zx)
            {
                zx.Log();
            }
        }

        /// <summary>
        /// متدی برای نمایش اعضای یک لیست از اشیاء در TableLayoutPanel به صورت ردیفی با هدر.
        /// هر ردیف شامل پراپرتی‌های دارای صفت PersianNameAttribute است و هدر شامل نام‌های فارسی پراپرتی‌ها.
        /// </summary>
        /// <param name="items">لیست یا آرایه‌ای از اشیاء</param>
        /// <param name="panel">TableLayoutPanel برای افزودن کنترل‌ها</param>
        public static void CreateAndAddPersianLabels(IEnumerable items, TableLayoutPanel panel)
        {
            try
            {
                if (items == null || panel == null) return;

                panel.SuspendLayout();
                panel.Controls.Clear();
                panel.RowStyles.Clear();
                panel.ColumnStyles.Clear();
                panel.RowCount = 0;
                panel.ColumnCount = 0;

                // دریافت پراپرتی‌های دارای صفت PersianNameAttribute از نوع اولین شیء
                var firstItem = items.Cast<object>().FirstOrDefault();
                if (firstItem == null) return;

                var props = firstItem.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<PersianNameAttribute>() != null)
                    .ToList();

                // تنظیم تعداد ستون‌ها
                panel.ColumnCount = props.Count;
                for (int i = 0; i < props.Count; i++)
                {
                    panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                }

                // افزودن ردیف هدر
                panel.RowCount = 1;
                panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                for (int colIndex = 0; colIndex < props.Count; colIndex++)
                {
                    var attr = props[colIndex].GetCustomAttribute<PersianNameAttribute>();
                    var headerLabel = new Label
                    {
                        Text = attr.Name,
                        Font = new Font("Tahoma", 8.25F, FontStyle.Bold),
                        ForeColor = SystemColors.ControlText,
                        RightToLeft = RightToLeft.Yes,
                        AutoSize = true,
                        Margin = new Padding(5, 4, 5, 4),
                        BackColor = SystemColors.ControlLight,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    panel.Controls.Add(headerLabel, colIndex, 0);
                }

                // افزودن ردیف‌های داده
                int rowIndex = 1;
                foreach (var item in items)
                {
                    if (item == null) continue;

                    panel.RowCount++;
                    panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                    for (int colIndex = 0; colIndex < props.Count; colIndex++)
                    {
                        var prop = props[colIndex];
                        var attr = prop.GetCustomAttribute<PersianNameAttribute>();
                        var value = prop.GetValue(item)?.ToString() ?? "";

                        var editTextBox = new ShafaCommon.UIControls.EditView.EditTextBox
                        {
                            Caption = attr.Name + ": ",
                            CaptionHeight = 14,
                            CaptionWidth = (attr.Name + ": ").Length * 8,
                            Name = prop.Name,
                            Value = Fn_ifNeed2Map(value, prop.Name),
                            ValueHeight = 21,
                            ValueWidth = value.Length * 8,
                            FontLable = new Font("Tahoma", 8.25F),
                            FontValue = new Font("Tahoma", 8.25F, FontStyle.Bold),
                            lbValueColor = SystemColors.WindowText,
                            RightToLeft = RightToLeft.Yes,
                            TextBoxRightToLeft = RightToLeft.Yes,
                            BackColor = SystemColors.ControlLightLight,
                            Size = new Size(
                                (int)(((value.Length * 8) + ((attr.Name + ": ").Length * 8)) * 1.3),
                                21
                            ),
                            Margin = new Padding(5, 4, 5, 4)
                        };

                        panel.Controls.Add(editTextBox, colIndex, rowIndex);
                    }
                    rowIndex++;
                }

                panel.ResumeLayout(true);
            }
            catch (Exception zx)
            {
                zx.Log();
            }
        }

        // ---------- نسخه جدید با Paging ----------
        public static void CreateAndAddPersianLabelsPaged(
            List<object> items, TableLayoutPanel panel,
            int pageSize, int pageIndex)
        {
            try
            {
                if (items == null || panel == null || items.Count == 0) return;

                // تنظیم هدر فقط در صفحه اول
                if (pageIndex == 0)
                {
                    panel.Controls.Clear();
                    panel.RowStyles.Clear();
                    panel.ColumnStyles.Clear();
                    var props = items[0].GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.GetCustomAttribute<PersianNameAttribute>() != null)
                        .ToList();
                    panel.ColumnCount = props.Count;
                    for (int i = 0; i < props.Count; i++)
                        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                    panel.RowCount = 1;
                    panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    for (int colIndex = 0; colIndex < props.Count; colIndex++)
                    {
                        var attr = props[colIndex].GetCustomAttribute<PersianNameAttribute>();
                        panel.Controls.Add(new Label
                        {
                            Text = attr.Name,
                            Font = new Font("Tahoma", 8.25F, FontStyle.Bold),
                            RightToLeft = RightToLeft.Yes,
                            AutoSize = true,
                            Margin = new Padding(5, 4, 5, 4),
                            BackColor = SystemColors.ControlLight
                        }, colIndex, 0);
                    }
                }

                var start = pageIndex * pageSize;
                var end = Math.Min(start + pageSize, items.Count);
                if (start >= end) return;

                var propsCached = items[0].GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<PersianNameAttribute>() != null)
                    .ToList();

                for (int i = start; i < end; i++)
                {
                    var item = items[i];
                    if (item == null) continue;
                    panel.RowCount++;
                    panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                    for (int colIndex = 0; colIndex < propsCached.Count; colIndex++)
                    {
                        var prop = propsCached[colIndex];
                        var attr = prop.GetCustomAttribute<PersianNameAttribute>();
                        var value = prop.GetValue(item)?.ToString() ?? "";
                        panel.Controls.Add(new ShafaCommon.UIControls.EditView.EditTextBox
                        {
                            Caption = attr.Name + ": ",
                            Name = prop.Name,
                            Value = Fn_ifNeed2Map(value, prop.Name),
                            FontLable = new Font("Tahoma", 8.25F),
                            FontValue = new Font("Tahoma", 8.25F, FontStyle.Bold),
                            RightToLeft = RightToLeft.Yes,
                            TextBoxRightToLeft = RightToLeft.Yes,
                            BackColor = SystemColors.ControlLightLight,
                            Size = new Size(
                                (int)(((value.Length * 8) + ((attr.Name + ": ").Length * 8)) * 1.3),
                                21
                            ),
                            Margin = new Padding(5, 4, 5, 4)
                        }, colIndex, panel.RowCount - 1);
                    }
                }
            }
            catch (Exception zx)
            {
                zx.Log();
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

        /// <summary>
        /// مخفی کردن سطر های جدول
        /// </summary>
        /// <param name="MyTbl">کنترل جدول</param>
        /// <param name="rows">ارایه حاوی شماره سطر های مد نظر برای مخفی شدن یا نشدن</param>
        /// <param name="isHide">مخفی بشود یا از حالت مخفی بیرون بیاد</param>
        public static void Fn_HideTableLayoutPanelInner_Row(TableLayoutPanel MyTbl, int[] rows, bool isHide)
        {
            foreach (Control control in MyTbl.Controls)
                foreach (int row in rows)
                    if (MyTbl.GetRow(control) == row)
                        control.Visible = isHide;
        }

        /// <summary>
        /// مخفی کردن سطر های جدول با ارتفاع
        /// </summary>
        /// <param name="MyTbl">کنترل جدول</param>
        /// <param name="rows">ارایه حاوی شماره سطر های مد نظر برای مخفی شدن یا نشدن</param>
        /// <param name="isHide">مخفی بشود یا از حالت مخفی بیرون بیاد</param>
        public static void Fn_HideTableLayoutPanel_Row(TableLayoutPanel MyTbl, int[] rows, bool isHide)
        {
            foreach (int row in rows)
                if (isHide)
                    MyTbl.RowStyles[row].Height = 0;
                else
                    MyTbl.RowStyles[row].Height = 1;
        }

        /// <summary>
        /// تبدیل رشته به تصویر
        /// </summary>
        /// <param name="base64String">base64String رشته برای کپچا یا عکس</param>
        /// <returns></returns>
        public static Image ConvertBase64ToImage(string base64String)
        {
            try
            {
                if (string.IsNullOrEmpty(base64String)) return null;
                // Remove the data URI prefix if present
                if (base64String.Contains(","))
                {
                    base64String = base64String.Split(',')[1];
                }

                // Convert base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(base64String);

                // Create memory stream from byte array
                using (var ms = new MemoryStream(imageBytes))
                {
                    // Create image from stream
                    Image image = Image.FromStream(ms);
                    return image;
                }
            }
            catch (Exception zx)
            {
                zx.Log();
                Console.WriteLine($"Error converting Base64 to Image: {zx.Message}");
                return null;
            }
        }

        /// <summary>
        /// خالی کردن کنترل ها و ردیف های درون جدول
        /// </summary>
        /// <param name="tableLayoutPanel"></param>
        public static void ClearTableLayoutPanel(TableLayoutPanel tableLayoutPanel)
        {
            try
            {
                tableLayoutPanel.SuspendLayout();
                // آزاد کردن منابع کنترل‌ها
                for (int i = tableLayoutPanel.Controls.Count - 1; i >= 0; i--)
                {
                    var control = tableLayoutPanel.Controls[i];
                    tableLayoutPanel.Controls.RemoveAt(i);
                    control.Dispose();
                }

                // پاک کردن تعریف سطرها و ستون‌ها (در صورت نیاز)
                //tableLayoutPanel.RowStyles.Clear();
                //tableLayoutPanel.ColumnStyles.Clear();
                tableLayoutPanel.RowCount = 0;
                tableLayoutPanel.ColumnCount = 0;

                tableLayoutPanel.ResumeLayout();
                tableLayoutPanel.Refresh();
            }
            catch (Exception zx)
            {
                zx.Log();
            }
        }

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
            return JsonConvert.SerializeObject(requestObj, Formatting.None,
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
        /// چک کردن وضعیت اینکه ایا موفق است یا خیر
        /// </summary>
        /// <param name="status"> مقدار عدد ورودی</param>
        /// <returns> خروجی بولین موفق یا ناموفق</returns>
        internal static bool ChechStatus(object myInstance)
        {
            if (myInstance == null)
                return false;
            // کلا گفتم با این متد هر پراپرتی ای به این نام داشتی با مقدار بیار
            int status = PopularStaticClass.GetPropertyValueSafe<int>(myInstance, "status");

            switch (status)
            {
                case 0:
                    //status == Sakhad_StaticInfoWebServiceData.Ws_SucsessStatus
                    return true;
                    break;

                case 1:
                    return false;
                    break;

                case 2:// زمانی که نیازه فرم وبسرویس خطای رفع محدودیت رو صدا بزنم
                    string captcha = ((UnlockByCaptchaVerificationResponse)myInstance).data.First().captcha;
                    new FrmVerifyCaptcha(captcha).ShowDialog();
                    return false;
                    break;
                //case 4:
                //    return true;
                //    break;
                default:

                    return false;
                    break;
            }
        }

        public static T Fn_FindParentOfType<T>(UserControl this_UC) where T : UserControl
        {
            Control current = this_UC.Parent;
            while (current != null)
            {
                if (current is T)
                    return (T)current;
                current = current.Parent;
            }
            return null;
        }

        public static T Fn_FindParentOfType<T>(Form this_UC) where T : UserControl
        {
            Control current = this_UC.Parent;
            while (current != null)
            {
                if (current is T)
                    return (T)current;
                current = current.Parent;
            }
            return null;
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

        public static void FnUI_lbl(Label lbl, bool isTrue)
        {
            try
            {
                if (isTrue)
                {
                    lbl.ForeColor = Color.White;
                    lbl.BackColor = Color.LimeGreen;
                }
                else
                {
                    lbl.ForeColor = Color.White;
                    lbl.BackColor = Color.Red;
                }
            }
            catch (Exception zx)
            {
                zx.Log();
            }
        }

        /// <summary>
        /// می‌خونه همه فایل‌های خدمات نسخه‌دار و بدون نسخه از روی دیسک، و نتایج رو در یک لیست تجمیع می‌کنه.
        /// </summary>
        /// <param name="basePath">مسیر پوشه حاوی فایل‌های JSON</param>
        /// <param name="serviceFiles">دایکشنری فایل‌های سرویس نسخه‌دار</param>
        /// <param name="unPrescribedFiles">دایکشنری فایل‌های خدمات بدون نسخه</param>
        /// <returns>لیست تجمیع‌شده از همه سرویس‌ها</returns>
        public static List<Inner_ServiceDataResponse> LoadAllServiceLists(
            string basePath,
            Dictionary<prescriptionType_308_enum, string> serviceFiles,
            Dictionary<prescriptionType_308_enum, string> unPrescribedFiles)
        {
            var allList = new List<Inner_ServiceDataResponse>();

            foreach (var kv in serviceFiles)
            {
                var fileName = kv.Value;
                if (string.IsNullOrWhiteSpace(fileName)) continue;

                var filePath = Path.Combine(basePath, $"json\\GetAllServiceListByType SumCount 231,854\\{fileName}");

                if (File.Exists(filePath))
                {
                    try
                    {
                        string json = File.ReadAllText(filePath);
                        var tempList = JsonConvert.DeserializeObject<List<Inner_ServiceDataResponse>>(json);

                        if (tempList != null)
                            allList.AddRange(tempList);
                    }
                    catch (Exception zx)
                    {
                        zx.Log();
                        Console.WriteLine($"[Error] در پردازش فایل {fileName}: {zx.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"[Skip] فایل یافت نشد: {fileName}");
                }
            }

            // حالا خدمات بدون نسخه هم اضافه می‌کنیم
            foreach (var kv in unPrescribedFiles)
            {
                var fileName = kv.Value;
                if (string.IsNullOrWhiteSpace(fileName)) continue;

                var filePath = Path.Combine(basePath, $"json\\getAllUnPrescribedServiceListByType SumCount 6,468\\{fileName}");

                if (File.Exists(filePath))
                {
                    try
                    {
                        string json = File.ReadAllText(filePath);
                        var tempList = JsonConvert.DeserializeObject<List<Inner_ServiceDataResponse>>(json);

                        if (tempList != null)
                            allList.AddRange(tempList);
                    }
                    catch (Exception zx)
                    {
                        zx.Log();
                        Console.WriteLine($"[Error] در پردازش فایل بدون نسخه {fileName}: {zx.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"[Skip] فایل UnPrescribed یافت نشد: {fileName}");
                }
            }

            Console.WriteLine($"✅ مجموع آیتم‌ها بارگذاری شد: {allList.Count}");
            return allList;
        }

        /// <summary>
        /// لود محتویات فایل به داخل کلاس
        /// </summary>
        /// <param name="fileName">اسم فایل</param>
        /// <param name="Is_unPrescribed">آیا غیر قابل تجویز است؟</param>
        /// <returns></returns>
        public static List<ServiceDataListCoustumized> LoadAllServiceLists(string fileName, bool Is_unPrescribed, string terminology_Code, string terminology_Name)
        {
            string basePath = Application.StartupPath;
            var allList = new List<ServiceDataListCoustumized>();

            if (!string.IsNullOrWhiteSpace(fileName) && terminology_Name.Trim().ToUpper() != "ERX")
            {
                var filePath = "";
                if (Is_unPrescribed)
                    filePath = Path.Combine(basePath, $"json\\getAllUnPrescribedServiceListByType SumCount 6,468\\{fileName}");
                else
                    filePath = Path.Combine(basePath, $"json\\GetAllServiceListByType SumCount 231,854\\{fileName}");

                if (File.Exists(filePath))
                {
                    try
                    {
                        string json = File.ReadAllText(filePath);
                        var tempList = JsonConvert.DeserializeObject<List<ServiceDataListCoustumized>>(json);

                        if (tempList != null)
                        {
                            FnLoopAddTErminology(tempList, terminology_Code, terminology_Name);

                            allList.AddRange(tempList);
                        }
                    }
                    catch (Exception zx)
                    {
                        zx.Log();
                        Console.WriteLine($"[Error] در پردازش فایل {fileName}: {zx.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"[Skip] فایل یافت نشد: {fileName}");
                }
            }

            Console.WriteLine($"✅ مجموع آیتم‌ها بارگذاری شد: {allList.Count}");
            return allList;
        }

        private static void FnLoopAddTErminology(List<ServiceDataListCoustumized> tempList, string terminology_Code, string terminology_Name)
        {
            try
            {
                if (tempList == null || tempList.Count == 0) return;

                for (int i = 0; i < tempList.Count; i++)
                {
                    var item = tempList[i];
                    if (item == null) continue;
                    item.terminology_Code = terminology_Code;
                    item.terminology_Name = terminology_Name;
                }
            }
            catch (Exception zx)
            {
                zx.Log();
            }
        }

        public static async Task<List<Inner_registerInitialPrescriptionRequest>> FnCheckToConvertTerminologyAndGetservice(List<Inner_registerInitialPrescriptionRequest> list_Inner_registerInitialPrescriptionRequest)
        {
            try
            {
                foreach (var item_inner in list_Inner_registerInitialPrescriptionRequest)
                {
                    string tr = item_inner.terminology;
                    var _type = SearchClass.FindByText<prescriptionType_308_enum>(tr.Trim());
                    prescriptionType_308_enum _typeTarget = _type.GetValueOrDefault();
                    // (prescriptionType_308_enum)Convert.ToInt32(item_inner.terminology);
                    var _response = new convertByTerminologyResponse();
                    var _request_Terminology = new convertByTerminologyRequest()
                    {
                        code = item_inner.code,
                        source = item_inner.terminology,// _type.ToString().Split('_').FirstOrDefault(),
                        target = ""
                    };

                    switch (_type)
                    {
                        case prescriptionType_308_enum.Generic:
                            _request_Terminology.target = "IRC";
                            _typeTarget = prescriptionType_308_enum.IRC_12;
                            break;

                        case prescriptionType_308_enum.RVU_1:
                            _request_Terminology.target = "LOINC";
                            _typeTarget = prescriptionType_308_enum.IRC_12;
                            break;

                        case prescriptionType_308_enum.ERX_2:
                            _request_Terminology.target = "IRC";
                            _typeTarget = prescriptionType_308_enum.IRC_12;
                            break;

                        case prescriptionType_308_enum.LOINC_3:
                            break;

                        case prescriptionType_308_enum.LOINC_4:
                            break;

                        case prescriptionType_308_enum.LOINC_6:
                            break;

                        case prescriptionType_308_enum.RVU_7:
                            break;

                        case prescriptionType_308_enum.SAKHAD_8:
                            break;

                        case prescriptionType_308_enum.SAKHAD_9:
                            break;

                        case prescriptionType_308_enum.IRC_12:
                            break;

                        case prescriptionType_308_enum.LOINC_13:
                            break;

                        case prescriptionType_308_enum.LOINC_14:
                            break;

                        case prescriptionType_308_enum.LOINC_15:
                            break;

                        case prescriptionType_308_enum.RVU_16:
                            break;

                        case prescriptionType_308_enum.SAKHAD_17:
                            break;

                        case prescriptionType_308_enum.SAKHAD_18:
                            break;

                        default:
                            break;
                    }
                    if (!string.IsNullOrEmpty(_request_Terminology.target))
                    {
                        _response = await CallWebSevice.FnconvertByTerminologyAsync(_request_Terminology);

                        if (_response != null &&
                            _response.data.FirstOrDefault() != null
                            )
                        {
                            var _data = _response.data.Where(x => x.basePrice != null && x.basePrice.Length > 0).FirstOrDefault();

                            var _request_service = new getServiceByTypeAndCodeRequest()
                            {
                                nationalNumber = _data.code,
                                type = _typeTarget.GetValue_String()
                            };
                            var _servic = await CallWebSevice.FnGetServiceByTypeAndCodeAsync(_request_service);
                            if (_servic != null &&
                                    _servic.data.FirstOrDefault() != null)
                            {
                                item_inner.code = _data.code;
                                item_inner.terminology = _request_Terminology.target;
                                item_inner.type = _typeTarget.GetValue_String();
                            }
                        }
                    }
                }
            }
            catch (Exception zx)
            {
                zx.Log();
            }
            return list_Inner_registerInitialPrescriptionRequest;
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

        /// <summary>
        /// تغییر سایز تصویر برای بهینه‌سازی حافظه
        /// </summary>
        internal static Image ResizeImage(Image originalImage, int maxWidth, int maxHeight)
        {
            try
            {
                // محاسبه نسبت ابعاد
                double ratioX = (double)maxWidth / originalImage.Width;
                double ratioY = (double)maxHeight / originalImage.Height;
                double ratio = Math.Min(ratioX, ratioY);

                int newWidth = (int)(originalImage.Width * ratio);
                int newHeight = (int)(originalImage.Height * ratio);

                Bitmap newImage = new Bitmap(newWidth, newHeight);

                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }

                return newImage;
            }
            catch
            {
                return originalImage; // در صورت خطا، تصویر اصلی رو برمی‌گردونه
            }
        }
    }
}