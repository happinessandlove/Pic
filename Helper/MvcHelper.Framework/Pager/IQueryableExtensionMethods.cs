/**************************************************
 * by 丁浩
 * 2016-01-30
 * **************************************************/

using System.Web.Mvc;
namespace System.Linq
{
    /// <summary>
    /// IQueryable&lt;T&gt;的扩展方法 分页相关
    /// </summary>
    public static partial class IQueryableExtensionMethods
    {
        /// <summary>
        /// （自定义）查询IQueryable&lt;TEntity&gt;实体集的指定页面数据。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <param name="query">IQueryable&lt;TEntity&gt;实体集。</param>
        /// <param name="pager">封装的页码信息</param>
        /// <returns></returns>
        public static IQueryable<TEntity> GetPageData<TEntity>(this IQueryable<TEntity> query, Pager pager) where TEntity : class
        {
            return query.Skip(pager.PageSize * (pager.PageIndex - 1)).Take(pager.PageSize).AsQueryable();
        }
    }
}
