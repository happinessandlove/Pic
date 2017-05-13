/**************************************************
 * by 丁浩
 * 2016-02-28
 * **************************************************/

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// Action的操作状态
    /// </summary>
    public enum ReturnType
    {
        /// <summary>
        /// 数据验证错误
        /// </summary>
        Invalid=1,
        /// <summary>
        /// 未登录
        /// </summary>
        NotLogin=2,
        /// <summary>
        /// 未授权
        /// </summary>
        NoPermission=3,
        /// <summary>
        /// 成功
        /// </summary>
        Success=4,
        /// <summary>
        /// 失败
        /// </summary>
        Failure=5,
        /// <summary>
        /// 错误
        /// </summary>
        Error=6,
        /// <summary>
        /// 其他
        /// </summary>
        Other=7,
        /// <summary>
        /// 添加成功
        /// </summary>
        CreateSuccess=8,

        /// <summary>
        /// 更新成功
        /// </summary>
        EditSuccess=9,

        /// <summary>
        /// 删除成功
        /// </summary>
        DeleteSuccess=10,

        /// <summary>
        /// 批量删除成功
        /// </summary>
        DeletesSuccess=11,

        /// <summary>
        /// 调整排序成功
        /// </summary>
        RankUpSuccess=12,

        /// <summary>
        /// 添加失败
        /// </summary>
        CreateFailure=13,

        /// <summary>
        /// 更新失败
        /// </summary>
        EditFailure=14,

        /// <summary>
        /// 删除失败
        /// </summary>
        DeleteFailure=15,

        /// <summary>
        /// 批量删除失败
        /// </summary>
        DeletesFailure=16,

        /// <summary>
        /// 调整排序失败
        /// </summary>
        RankUpFailure=17
    }


    /// <summary>
    /// Action返回值的封装类型
    /// <para>可用于Ajax访问方法反馈结果</para>
    /// </summary>
    public class ReturnValue
    {
        /// <summary>
        /// 操作结果类型
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ReturnType Type { get; set; }

        /// <summary>
        /// 操作结果提示
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 操作结果携带数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReturnValue()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">操作结果类型</param>
        /// <param name="message">操作结果提示</param>
        /// <param name="data">操作结果携带数据</param>
        public ReturnValue(ReturnType type, string message,object data)
        {
            this.Type = type;
            this.Message = message;
            this.Data = data;
        }

        /// <summary>
        /// 将返回值序列化为Json字符串
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });
        }
    }
}