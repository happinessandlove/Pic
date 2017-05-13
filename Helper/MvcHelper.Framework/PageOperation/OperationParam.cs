/**************************************************
 * by 丁浩
 * 2015-02-10 
 * **************************************************/

using System.Globalization;
using System.Web.Helpers;

namespace System.Web.Mvc
{
    /// <summary>
    /// （自定义）页面操作封装类
    /// </summary>
    public class OperationParam
    {
        /// <summary>
        /// 操作类型。OperationType枚举
        /// </summary>
        public OperationType OpType { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public string OpArgument { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int OpPager { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OpSortProperty { get; set; }
        /// <summary>
        /// 排序方向。OrderDirection枚举
        /// </summary>
        public SortDirection OpSortDirection { get; set; }
        /// <summary>
        /// 查询条件序列化字符串
        /// </summary>
        public string OpQueryString { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public OperationParam()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="sortProperty">排序属性名称，缺省值为升序</param>
        /// <param name="sortDirection">排序方向，SortDirection枚举</param>
        public OperationParam(string sortProperty, SortDirection sortDirection = SortDirection.Ascending, int pageIndex = 1)
        {
            this.OpPager = pageIndex;
            this.OpType = OperationType.Null;
            this.OpArgument = "";
            this.OpQueryString = "";
            this.OpSortProperty = sortProperty;
            this.OpSortDirection = sortDirection;
        }

        /// <summary>
        /// 重置操作参数文本框的值
        /// </summary>
        /// <param name="modelState"></param>
        public static void Reset(ModelStateDictionary modelState)
        {
            modelState["OpType"].Value = new ValueProviderResult(OperationType.Null, "0", CultureInfo.CurrentCulture);
        }
    }
}