/**************************************************
 * by 丁浩
 * 2016-02-02 
***************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{
    //[MyRequired]
    #region 必需 继承Required
    /// <summary>
    /// 必需验证
    /// <para>  数值非空属性必须加此标识，否则验证的错误信息为默认的</para>
    /// </summary>
    public sealed class MyRequiredAttribute : RequiredAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyRequiredAttribute()
        {
            base.ErrorMessage = "请输入{0}";
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "required", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyChecked]
    #region 复选框选中
    /// <summary>
    /// 复选框选中验证
    /// </summary>
    public sealed class MyCheckedAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyCheckedAttribute()
        {
            ErrorMessage = "请选择{0}";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            return (bool)value;
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "checked", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyDate]
    #region 日期 继承DataType
    /// <summary>
    /// 日期验证 
    /// </summary>
    public sealed class MyDateAttribute : DataTypeAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyDateAttribute()
            : base(DataType.Date)
        {
            base.ErrorMessage = "请输入正确格式的{0}（如:" + DateTime.Now.ToString("yyyy-MM-dd") + "）";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            return true;
            //if (value == null) return true;
            //DateTime time;
            //DateTime.TryParse(Convert.ToString(value), out time);
            //return (time != new DateTime());
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "datecn", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyYearMonth]
    #region 年月
    /// <summary>
    /// 年月验证 格式为：四位年份+.或/或-+一或两位月份
    /// </summary>    
    public sealed class MyYearMonthAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyYearMonthAttribute()
        {
            ErrorMessage = base.ErrorMessage = "请输入正确格式的{0}（如:1990.12）";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                Regex pattern = new Regex(@"^\d{4}[\.\/-]\d{1,2}$");
                if (pattern.IsMatch(value.ToString()))
                {
                    var d = value.ToString().Split(new string[] { "-", ".", "/" }, StringSplitOptions.RemoveEmptyEntries);
                    if (int.Parse(d[1]) < 13 && int.Parse(d[1]) != 0) return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "yearmonth", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyPostalCode]
    #region 邮政编码
    /// <summary>
    /// 邮政编码验证
    /// </summary>
    public sealed class MyPostalCodeAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyPostalCodeAttribute()
        {
            ErrorMessage = "请输入正确格式的{0}";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                Regex pattern = new Regex(@"^[1-9]\d{5}$");
                return pattern.IsMatch(value.ToString());
            }
            catch
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "postalcode", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyEmailAddress]
    #region 邮箱地址
    /// <summary>
    /// 邮箱地址验证
    /// </summary>
    public sealed class MyEmailAddressAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyEmailAddressAttribute()
        {
            ErrorMessage = "请输入正确格式的{0}";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                Regex pattern = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                return pattern.IsMatch(value.ToString());
            }
            catch
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "email", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyTelePhone]
    #region 固定电话
    /// <summary>
    /// 固定电话验证
    /// </summary>
    public sealed class MyTelePhoneAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyTelePhoneAttribute()
        {
            ErrorMessage = "请输入正确格式的固定电话（如：010-12345678）";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                Regex pattern = new Regex(@"^(\(0\d{2,3}\)|(0\d{2,3}-))?[2-9]\d{6,7}(-\d{1,4}|\(\d{1,4}\))?$");
                return pattern.IsMatch(value.ToString());
            }
            catch
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "telephone", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyMobile]
    #region 移动电话
    /// <summary>
    /// 移动电话验证
    /// </summary>
    public sealed class MyMobileAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyMobileAttribute()
        {
            ErrorMessage = "请输入正确格式的手机号码";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                Regex pattern = new Regex(@"^1\d{10}$");
                return pattern.IsMatch(value.ToString());
            }
            catch
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "mobile", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyPhone]
    #region 固定电话或移动电话
    /// <summary>
    /// 固定电话或移动电话验证
    /// </summary>
    public sealed class MyPhoneAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyPhoneAttribute()
        {
            ErrorMessage = "请输入正确格式的固定电话或移动电话";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                Regex pattern = new Regex(@"(^1\d{10}$)|(^(\(0\d{2,3}\)|(0\d{2,3}-))?[2-9]\d{6,7}(-\d{1,4}|\(\d{1,4}\))?$)");
                return (pattern.IsMatch(value.ToString()));
            }
            catch
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "phone", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyFax]
    #region 传真
    /// <summary>
    /// 传真验证
    /// </summary>
    public sealed class MyFaxAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyFaxAttribute()
        {
            ErrorMessage = "请输入正确格式的{0}";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                Regex pattern = new Regex(@"^(\(0\d{2,3}\)|(0\d{2,3}-))?[2-9]\d{6,7}$");
                return pattern.IsMatch(value.ToString());
            }
            catch
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "fax", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyUrl]
    #region 网址
    /// <summary>
    /// 网址验证
    /// </summary>
    public sealed class MyUrlAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyUrlAttribute()
        {
            ErrorMessage = "请输入正确格式的{0}";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                Regex pattern = new Regex(@"^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                return pattern.IsMatch(value.ToString());
            }
            catch
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "url", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyAccept("jpg,png,gif")]
    #region 文件后缀名
    /// <summary>
    /// 文件后缀名验证
    /// </summary>
    public sealed class MyAcceptAttribute : ValidationAttribute, IClientValidatable
    {
        private string exts;
        /// <summary>
        /// 文件后缀名验证
        /// </summary>
        /// <param name="exts">允许的后缀名，多个后缀名以逗号隔开，如："jpg,png,gif"</param>
        public MyAcceptAttribute(string exts)
        {
            this.exts = exts.ToLowerInvariant();
            ErrorMessage = "请上传正确的文件格式（" + exts + "）";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                return this.exts.Split(',').Contains<string>(Path.GetExtension(value.ToString()).Replace(".", "").ToLowerInvariant());
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "accept", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            rule.ValidationParameters.Add("exts", exts);
            yield return rule;
        }
    }
    #endregion

    //[MyMaxLength(10)]
    #region 字符串最大长度
    /// <summary>
    /// 字符串最大长度验证，同时决定数据库字段的长度
    /// </summary>
    public sealed class MyMaxLengthAttribute : MaxLengthAttribute, IClientValidatable
    {
        private int maxLength;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxLength">最大长度</param>
        public MyMaxLengthAttribute(int maxLength)
            : base(maxLength)
        {
            this.maxLength = maxLength;
            base.ErrorMessage = "{0}不超过" + maxLength + "个字符";
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "length", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            rule.ValidationParameters.Add("max", maxLength);
            yield return rule;
        }
    }
    #endregion

    //[MyMinLength(10)]
    #region 字符串最小长度
    /// <summary>
    /// 字符串最小长度验证
    /// </summary>
    public sealed class MyMinLengthAttribute : MinLengthAttribute, IClientValidatable
    {
        private int minLength;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minLength">最小长度</param>
        public MyMinLengthAttribute(int minLength)
            : base(minLength)
        {
            this.minLength = minLength;
            ErrorMessage = "{0}至少" + minLength + "个字符";
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "length", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            rule.ValidationParameters.Add("min", minLength);
            yield return rule;
        }
    }
    #endregion

    //仅限定最小长度[MyStringLength(10)]
    //仅限定最大长度[MyStringLength(0, 10)]
    //限定最小与最大长度[MyStringLength(2, 10)]
    #region 字符串长度范围
    /// <summary>
    /// 字符串长度范围验证，限定最大长度时将决定数据库字段的长度
    /// </summary>
    public sealed class MyStringLengthAttribute : StringLengthAttribute, IClientValidatable
    {
        private int minLength;
        private int maxLength;
        /// <summary>
        /// 仅限定最小长度 
        /// </summary>
        /// <param name="minLength">最小长度</param>
        public MyStringLengthAttribute(int minLength)
            : base(int.MaxValue)
        {
            base.MinimumLength = minLength;
            this.minLength = minLength;
            this.maxLength = int.MaxValue;
            ErrorMessage = "{0}至少" + minLength + "个字符";
        }
        /// <summary>
        /// 限定最小与最大长度
        /// </summary>
        /// <param name="minLength">最小长度</param>
        /// <param name="maxLength">最大长度</param>
        public MyStringLengthAttribute(int minLength, int maxLength)
            : base(maxLength)
        {
            base.MinimumLength = minLength;
            this.minLength = minLength;
            this.maxLength = maxLength;
            if (minLength == 0) ErrorMessage = "{0}不超过" + maxLength + "个字符";
            else ErrorMessage = "{0}介于" + minLength + "至" + maxLength + "个字符之间";
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = null;
            if (minLength == 0 && maxLength != int.MaxValue)
            {
                rule = new ModelClientValidationRule { ValidationType = "length", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
                rule.ValidationParameters.Add("max", maxLength);
            }
            else if (minLength != 0 && maxLength == int.MaxValue)
            {
                rule = new ModelClientValidationRule { ValidationType = "length", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
                rule.ValidationParameters.Add("min", minLength);
            }
            else
            {
                rule = new ModelClientValidationRule { ValidationType = "length", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
                rule.ValidationParameters.Add("max", maxLength);
                rule.ValidationParameters.Add("min", minLength);
            }
            yield return rule;
        }
    }
    #endregion

    //[MyStringLengthChs]
    #region 字符串长度范围 中文算两位
    /// <summary>
    /// 字符串长度范围验证，中文算两位。
    /// <para>  不决定数据库字段长度</para>
    /// </summary>
    public sealed class MyStringLengthChsAttribute : ValidationAttribute, IClientValidatable
    {
        private int minLength;
        private int maxLength;
        /// <summary>
        /// 仅限定最小长度 
        /// </summary>
        /// <param name="minLength">最小长度</param>
        public MyStringLengthChsAttribute(int minLength)
        {
            this.minLength = minLength;
            this.maxLength = int.MaxValue;
            ErrorMessage = "{0}至少" + minLength + "个字符，一个中文为2个字符";
        }
        /// <summary>
        /// 限定最小与最大长度
        /// </summary>
        /// <param name="minLength">最小长度</param>
        /// <param name="maxLength">最大长度</param>
        public MyStringLengthChsAttribute(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
            if (minLength == 0) ErrorMessage = "{0}不超过" + maxLength + "个字符，一个中文为2个字符";
            else ErrorMessage = "{0}介于" + minLength + "至" + maxLength + "个字符之间，一个中文为2个字符";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                Regex pattern = new Regex(@"[^\x00-\xff]");
                int length = pattern.Replace(value.ToString(), "**").Length;
                if (length >= minLength && length <= maxLength) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "chslength", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            rule.ValidationParameters.Add("max", maxLength);
            rule.ValidationParameters.Add("min", minLength);
            yield return rule;
        }
    }
    #endregion

    //[MyFixedLenngth(10)]
    #region 字符串固定长度
    /// <summary>
    /// 字符串固定长度验证，同时决定数据库字段的长度
    /// </summary>
    public sealed class MyFixedLengthAttribute : StringLengthAttribute, IClientValidatable
    {
        private int length;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length">长度</param>
        public MyFixedLengthAttribute(int length)
            : base(length)
        {
            this.length = length;
            base.MinimumLength = length;
            base.ErrorMessage = "{0}应为" + length + "个字符";
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "fixedlength", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            rule.ValidationParameters.Add("length", length);
            yield return rule;
        }
    }
    #endregion

    //[MyInteger]
    #region 整数 正整数、负整数，限定正整数请用Min或Range
    /// <summary>
    /// 整数验证 正整数、负整数，限定正整数请用Min或Range
    /// </summary>
    public sealed class MyIntegerAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyIntegerAttribute()
        {
            ErrorMessage = "请输入整数";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            return true;
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "integer", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyNumber]
    #region 数字 正数、负数，可含小数，不可用在数值类型属性上，限定正数请用Min或Range
    /// <summary>
    /// 数字验证
    /// <para>  数字 正数、负数，可含小数，不可用在数值类型属性上，限定正数请用Min或Range</para>
    /// </summary>
    public sealed class MyNumberAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyNumberAttribute()
        {
            ErrorMessage = "请输入数字";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            long num;
            return ((value == null) || (long.TryParse(Convert.ToString(value), out num) && (num >= 0L)));
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "number", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyMax]
    #region 最大值
    /// <summary>
    /// 数值型最大值
    /// </summary>
    public sealed class MyMaxAttribute : RangeAttribute, IClientValidatable
    {
        private bool isInt;
        private int maxi;
        private double maxd;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="max">最大值</param>
        public MyMaxAttribute(int max)
            : base(int.MinValue, max)
        {
            maxi = max;
            isInt = true;
            ErrorMessage = "请输入不超过" + max + "的数值";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="max">最大值</param>
        public MyMaxAttribute(double max)
            : base(double.MinValue, max)
        {
            maxd = max;
            isInt = false;
            ErrorMessage = "请输入不超过" + max + "的数值";
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = null;
            rule = new ModelClientValidationRule { ValidationType = "range", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };

            if (isInt) rule.ValidationParameters.Add("max", maxi);
            else rule.ValidationParameters.Add("max", maxd);
            yield return rule;
        }
    }
    #endregion

    //[MyMin]
    #region 最小值
    /// <summary>
    /// 最小值验证
    /// </summary>
    public sealed class MyMinAttribute : RangeAttribute, IClientValidatable
    {
        private bool isInt;
        private int mini;
        private double mind;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">最小值</param>
        public MyMinAttribute(int min)
            : base(min, int.MaxValue)
        {
            mini = min;
            isInt = true;
            ErrorMessage = "请输入不小于" + min + "的数值";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">最小值</param>
        public MyMinAttribute(double min)
            : base(min, double.MaxValue)
        {
            mind = min;
            isInt = false;
            ErrorMessage = "请输入不小于" + min + "的数值";
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = null;
            rule = new ModelClientValidationRule { ValidationType = "range", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };

            if (isInt) rule.ValidationParameters.Add("min", mini);
            else rule.ValidationParameters.Add("min", mind);
            yield return rule;
        }
    }
    #endregion

    //[MyRange]
    #region 数值范围
    /// <summary>
    /// 数值范围
    /// </summary>
    public sealed class MyRangeAttribute : RangeAttribute, IClientValidatable
    {
        private bool isInt;
        private int maxi;
        private int mini;
        private double maxd;
        private double mind;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public MyRangeAttribute(int min, int max)
            : base(min, max)
        {
            maxi = max;
            mini = min;
            isInt = true;
            ErrorMessage = "请输入介于" + min + "与" + max + "之间的数值";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public MyRangeAttribute(double min, double max)
            : base(min, max)
        {
            maxd = max;
            mind = min;
            isInt = false;
            ErrorMessage = "请输入介于" + min + "与" + max + "之间的数值";
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = null;
            rule = new ModelClientValidationRule { ValidationType = "range", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };

            if (isInt)
            {
                rule.ValidationParameters.Add("max", maxi);
                rule.ValidationParameters.Add("min", mini);
            }
            else
            {
                rule.ValidationParameters.Add("max", maxd);
                rule.ValidationParameters.Add("min", mind);
            }
            yield return rule;
        }
    }
    #endregion

    //[MySelect]
    #region 下拉列表框
    /// <summary>
    /// 下拉列表框验证 值为0表示未选
    /// </summary>
    public sealed class MySelectAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MySelectAttribute()
        {
            ErrorMessage = ErrorMessage ?? "请选择{0}";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (value.ToString() == "--请选择--") return false;
            return (value.ToString() != "0");
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "selectoption", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyIdCardNo]
    #region 身份证号
    /// <summary>
    /// 身份证号验证
    /// </summary>
    public sealed class MyIdCardNoAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyIdCardNoAttribute()
        {
            ErrorMessage = "请输入正确格式的身份证号";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            string Id = (string)value;
            if (Id.Length == 18) return CheckIDCardNo18(Id);
            else if (Id.Length == 15) return CheckIDCardNo15(Id);
            else return false;
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "idcardno", ErrorMessage = ErrorMessage };
            yield return rule;
        }

        private static bool CheckIDCardNo18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false; //数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false; //省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime birthday = new DateTime();
            //DateTime now = DateTime.Now;
            if (DateTime.TryParse(birth, out birthday) == false)
                return false; //日期验证
            //if (birthday >= now.AddYears(-10) || birthday <= now.AddYears(-150))
            //    return false; //出生日期应介于[当前时间-150，当前时间-10]之间
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false; //校验码验证
            }
            return true; //符合GB11643-1999标准
        }

        private static bool CheckIDCardNo15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false; //数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false; //省份验证
            }
            string birth = "19" + Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");//15位均认为年份前两位为19
            DateTime birthday = new DateTime();
            //DateTime now = DateTime.Now;
            if (DateTime.TryParse(birth, out birthday) == false)
                return false; //日期验证
            //if (birthday >= now.AddYears(-10) || birthday <= now.AddYears(-150))
            //    return false; //出生日期应介于[当前时间-150，当前时间-10]之间
            return true; //符合15位身份证标准
        }
    }
    #endregion

    //[MyCompare(字段名称,比较形式,数据类型,提示信息)]
    #region 比较
    /// <summary>
    /// 比较 客户端
    /// </summary>
    public class ModelClientValidationMyCompareRule : ModelClientValidationRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="Original"></param>
        /// <param name="op"></param>
        /// <param name="type"></param>
        public ModelClientValidationMyCompareRule(string errorMessage, string Original, ValidationCompareOperator op, string type)
        {
            ValidationType = "compareto";
            ErrorMessage = errorMessage;
            string oprtor = "";
            switch (op)
            {
                case ValidationCompareOperator.Equal: oprtor = "=="; break;
                case ValidationCompareOperator.GreaterThan: oprtor = ">"; break;
                case ValidationCompareOperator.GreaterThanEqual: oprtor = ">="; break;
                case ValidationCompareOperator.LessThan: oprtor = "<"; break;
                case ValidationCompareOperator.LessThanEqual: oprtor = "<="; break;
                case ValidationCompareOperator.NotEqual: oprtor = "!="; break;
            }
            ValidationParameters["original"] = Original;
            ValidationParameters["op"] = oprtor;
            ValidationParameters["type"] = type;
        }
    }
    /// <summary>
    /// 比较验证的属性类型
    /// </summary>
    public enum ValidationDataType : byte
    {
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// 整型
        /// </summary>
        Integer,
        /// <summary>
        /// 浮点型
        /// </summary>
        Double,
        /// <summary>
        /// 日期时间
        /// </summary>
        Date,
        /// <summary>
        /// 货币
        /// </summary>
        Currency,
        /// <summary>
        /// 高精度浮点型
        /// </summary>
        Decimal
    }
    /// <summary>
    /// 比较验证的关系类型
    /// </summary>
    public enum ValidationCompareOperator : byte
    {
        /// <summary>
        /// ==
        /// </summary>
        Equal,
        /// <summary>
        /// !=
        /// </summary>
        NotEqual,
        /// <summary>
        /// &gt;
        /// </summary>
        GreaterThan,
        /// <summary>
        /// &gt;=
        /// </summary>
        GreaterThanEqual,
        /// <summary>
        /// &lt;
        /// </summary>
        LessThan,
        /// <summary>
        /// &lt;=
        /// </summary>
        LessThanEqual
    }
    /// <summary>
    /// 两个字段（属性）的比较验证
    /// 验证信息必须明确指定
    /// </summary>
    /// <returns></returns>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class MyCompareAttribute : ValidationAttribute, IClientValidatable
    {
        private string originalProperty;
        private ValidationCompareOperator op;
        private ValidationDataType type;
        private const string _defaultErrorMessage = "'{0}'与 '{1}' 进行{2}比较失败";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalProperty">相比较的属性名称</param>
        /// <param name="op">关系。ValidationCompareOperator枚举值</param>
        /// <param name="type">数据类型。ValidationDataType枚举值</param>
        public MyCompareAttribute(string originalProperty, ValidationCompareOperator op, ValidationDataType type)
            : base(_defaultErrorMessage)
        {
            this.originalProperty = originalProperty;
            this.op = op;
            this.type = type;
        }
        /// <summary> </summary>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name, originalProperty, op.ToString());
        }
        /// <summary> </summary>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo originalProperty = ReflectionHelper.GetProperty(validationContext.ObjectType, this.originalProperty);
            object originalValue = originalProperty.GetValue(validationContext.ObjectInstance, null);
            if (!Compare(value, originalValue))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            return ValidationResult.Success;
        }
        private bool Compare(object sourceProperty, object originalProperty)
        {
            if (sourceProperty == null || originalProperty == null) return true;
            int num = 0;
            switch (this.type)
            {
                case ValidationDataType.String:
                    num = string.Compare((string)sourceProperty, (string)originalProperty, false, CultureInfo.CurrentCulture);
                    break;
                case ValidationDataType.Integer:
                    num = ((int)sourceProperty).CompareTo(originalProperty);
                    break;
                case ValidationDataType.Double:
                    num = ((double)sourceProperty).CompareTo(originalProperty);
                    break;
                case ValidationDataType.Date:
                    num = ((DateTime)sourceProperty).CompareTo(originalProperty);
                    break;
                case ValidationDataType.Currency:
                    num = ((decimal)sourceProperty).CompareTo(originalProperty);
                    break;
            }
            switch (this.op)
            {
                case ValidationCompareOperator.Equal:
                    return (num == 0);
                case ValidationCompareOperator.NotEqual:
                    return (num != 0);
                case ValidationCompareOperator.GreaterThan:
                    return (num > 0);
                case ValidationCompareOperator.GreaterThanEqual:
                    return (num >= 0);
                case ValidationCompareOperator.LessThan:
                    return (num < 0);
                case ValidationCompareOperator.LessThanEqual:
                    return (num <= 0);
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationMyCompareRule(FormatErrorMessage(metadata.GetDisplayName()), originalProperty, op, type.ToString());
            yield return rule;
        }
    }
    #endregion

    //[MyUserName]
    #region 用户名 （中文、字母、数字、.-_@）
    /// <summary>
    /// 用户名验证
    /// <para>  由中文、字母、数字、.-_@组成</para>
    /// </summary>
    public sealed class MyUserNameAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyUserNameAttribute()
        {
            ErrorMessage = "{0}由中文、字母、数字或特殊字符（. _ - @）组成，且只能以中文、字母或数字开头";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                Regex regex = new Regex(@"^[0-9a-zA-Z\u4e00-\u9fa5][@0-9a-zA-Z\._\-\u4e00-\u9fa5]*$");
                return regex.Match(value.ToString()).Success;
            }
            catch
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "username", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion

    //[MyPassword]
    #region 密码 （字母、数字、下划线组成，必须包含其中两种字符）
    /// <summary>
    /// 密码验证
    /// <para>  由字母、数字、下划线组成，必须包含其中两种字符</para>
    /// </summary>
    public sealed class MyPasswordAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary> </summary>
        public MyPasswordAttribute()
        {
            ErrorMessage = "{0}由字母、数字或下划线三类字符组成，且必须包含其中两类";
        }
        /// <summary> </summary>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            try
            {
                Regex regex = new Regex(@"^\w+$");
                if (!regex.Match(value.ToString()).Success) return false;
                Regex reg1 = new Regex(@"^(?![^a-zA-Z]+$)(?!\D+$).*$");
                Regex reg2 = new Regex(@"^(?![^a-zA-Z]+$)(?![^_]+$).*$");
                Regex reg3 = new Regex(@"^(?!\D+$)(?![^_]+$).*$");
                if (!reg1.Match(value.ToString()).Success && !reg2.Match(value.ToString()).Success && !reg3.Match(value.ToString()).Success)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary> </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule { ValidationType = "pwd", ErrorMessage = base.FormatErrorMessage(metadata.GetDisplayName()) };
            yield return rule;
        }
    }
    #endregion
}