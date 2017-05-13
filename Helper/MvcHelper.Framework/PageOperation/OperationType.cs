/**************************************************
 * by 丁浩
 * 2015-02-04 
 * **************************************************/

namespace System.Web.Mvc
{
    /// <summary>
    /// （自定义）列表页提交表单的操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 未填写，默认值
        /// </summary>
        Null = 0,

        /// <summary>
        /// 翻页
        /// </summary>
        Pager,

        /// <summary>
        /// 查询
        /// </summary>
        Query,

        /// <summary>
        /// 排序
        /// </summary>
        Sort,

        /// <summary>
        /// 删除
        /// </summary>
        Delete,

        /// <summary>
        /// 批量删除
        /// </summary>
        Deletes,

        /// <summary>
        /// 显示全部
        /// </summary>
        ShowAll,

        /// <summary>
        /// 调整排序
        /// </summary>
        RankUp
    }
}
