/**************************************************
 * by 丁浩
 * 2015-03-25 
 * **************************************************/

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// （自定义）查询对话框的HtmlHelper扩展方法
    /// </summary>
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// 扩展方法，查询对话框。
        /// </summary>
        /// <typeparam name="TModel">视图页面强类型</typeparam>
        /// <param name="html"></param>
        /// <param name="queryString">json格式的查询字符串。用于查询后的页面生成时，重绘先前的查询对话框中的查询条件。</param>
        /// <param name="excludeProperties">排除的属性，多个属性以逗号隔开</param>
        /// <returns></returns>
        public static MvcHtmlString QueryFor<TModel>(this HtmlHelper<TModel> html, string queryString = null, string excludeProperties = null)
        {
            Type type = typeof(TModel);
            if (type.IsGenericType) type = type.GenericTypeArguments[0];
            return QueryHelper.RenderQuery(type, queryString, excludeProperties);
        }
    }
}
