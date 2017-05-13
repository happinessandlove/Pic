/**************************************************
 * by 丁浩
 * 2016-01-30 
 * **************************************************/

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// （自定义）查询字段的类型。
    /// </summary>
    public enum QueryPropertyType
    {
        /// <summary>
        /// 性别
        /// </summary>
        Gender, 

        /// <summary>
        /// 字符串
        /// </summary>
        String, 
        
        /// <summary>
        /// 数值
        /// </summary>
        Numerical, 
        
        /// <summary>
        /// 日期时间
        /// </summary>
        DateTime, 
        
        /// <summary>
        /// 逻辑
        /// </summary>
        Boolean, 
        
        /// <summary>
        /// 下拉列表框选择
        /// </summary>
        Select,

        /// <summary>
        /// 枚举
        /// </summary>
        Enum
    }
}