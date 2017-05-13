using System.Collections.Generic;
using System.Linq;
using System.Text;

/**************************************************
 * v1.1
 * by 丁浩
 * 2014-05-29
 * **************************************************/

namespace System.Web.Mvc
{
    /// <summary>
    /// Action的操作状态
    /// </summary>
    public enum ReturnType
    {
        /// <summary>
        /// 数据验证错误
        /// </summary>
        Invalid, 
        /// <summary>
        /// 未登录
        /// </summary>
        NotLogin, 
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 失败
        /// </summary>
        Failure,
        /// <summary>
        /// 错误
        /// </summary>
        Error, 
        /// <summary>
        /// 未授权
        /// </summary>
        NoPermission,
        /// <summary>
        /// 其他
        /// </summary>
        Other
    }

    /// <summary>
    /// Action返回值数据项的封装类型
    /// </summary>
    public class ReturnItem
    {
        /// <summary>
        /// 项的名称
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 项的值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Value值序列化时是否添加双引号
        /// </summary>
        public bool AddQuote { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">项的名称</param>
        /// <param name="value">项的值</param>
        public ReturnItem(string key, object value)
        {
            this.Key = key;
            this.Value = value;
            Type type = Value.GetType();
            if (type.Equals(typeof(int)) || type.Equals(typeof(float)) || type.Equals(typeof(double)) || type.Equals(typeof(decimal)))
                AddQuote = false;
            else if (type.Equals(typeof(string)))
            {
                if ((value.ToString().Contains("[") && value.ToString().Contains("]")) || (value.ToString().Contains("{") && value.ToString().Contains("}")))
                    AddQuote = false;
                else AddQuote = true;
            }
            else AddQuote = true;
        }
    }

    /// <summary>
    /// Action返回值的封装类型，继承List&lt;ReturnItem&gt;
    /// <para>  可用于Ajax访问方法反馈结果</para>
    /// </summary>
    public class ReturnValue : List<ReturnItem>
    {
        private ReturnType returnType;
        /// <summary>
        /// 操作结果
        /// </summary>
        public ReturnType ReturnType
        {
            get
            {
                return this.returnType;
            }
            set
            {
                this.returnType = value; this.Add(new ReturnItem("result", this.ReturnType.ToString()));
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReturnValue()
        { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="returnType">操作结果</param>
        public ReturnValue(ReturnType returnType)
        {
            this.ReturnType = returnType;
            this.Add(new ReturnItem("result", this.ReturnType.ToString()));
        }

        /// <summary>
        /// 将返回值序列化为Json字符串
        /// </summary>
        /// <param name="valueIsJson"></param>
        /// <returns></returns>
        public string ToJson(bool valueIsJson = false)
        {
            StringBuilder sb = new StringBuilder("{");
            sb.Append(string.Join(",", this.Select(t => "\"" + t.Key + "\":" + (t.AddQuote ? "\"" : "") + (t.AddQuote ? t.Value.ToString() : t.Value) + (t.AddQuote ? "\"" : ""))))
            .Append("}");
            return sb.ToString();
        }
    }
}