/**************************************************
 * by 丁浩
 * 2015-02-10
 * **************************************************/

using Newtonsoft.Json;
using System.Collections.Generic;

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
        Other,
        /// <summary>
        /// 添加成功
        /// </summary>
        CreateSuccess,

        /// <summary>
        /// 更新成功
        /// </summary>
        EditSuccess,

        /// <summary>
        /// 删除成功
        /// </summary>
        DeleteSuccess,

        /// <summary>
        /// 批量删除成功
        /// </summary>
        DeletesSuccess,

        /// <summary>
        /// 调整排序成功
        /// </summary>
        RankUpSuccess,

        /// <summary>
        /// 添加失败
        /// </summary>
        CreateFailure,

        /// <summary>
        /// 更新失败
        /// </summary>
        EditFailure,

        /// <summary>
        /// 删除失败
        /// </summary>
        DeleteFailure,

        /// <summary>
        /// 批量删除失败
        /// </summary>
        DeletesFailure,

        /// <summary>
        /// 调整排序失败
        /// </summary>
        RankUpFailure
    }


    /// <summary>
    /// Action返回值的封装类型，继承Dictionary&lt;ReturnItem&gt;
    /// <para>可用于Ajax访问方法反馈结果</para>
    /// </summary>
    public class ReturnValue : Dictionary<string,string>
    {
        private ReturnType returnType;
        private string message;
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
                this.returnType = value; this.Add("result", this.returnType.ToString());
            }
        }
        /// <summary>
        /// 操作结果提示
        /// </summary>
        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value; this.Add("message", this.message);
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
            this.Add("result", this.ReturnType.ToString());
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="returnType">操作结果</param>
        /// <param name="message">操作结果提示</param>
        public ReturnValue(ReturnType returnType, string message)
        {
            this.ReturnType = returnType;
            this.Add("result", this.ReturnType.ToString());
            this.Add("message", this.message);
        }

        /// <summary>
        /// 将返回值序列化为Json字符串
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}