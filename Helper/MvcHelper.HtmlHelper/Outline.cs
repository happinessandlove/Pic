/**************************************************
 * by 丁浩
 * 2015-01-31 
***************************************************/

using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// HtmlHelper的扩展方法，用于呈现Html元素。
    /// </summary>
    public static partial class HtmlHelpers
    {        
        /// <summary>
        /// （自定义）截取内容的前若干个字符
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="length">长度  一个中文算1个长度</param>
        /// <returns></returns>
        public static MvcHtmlString OutlineFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int length)
        {
            string text = html.DisplayFor(expression).ToString();
            int len = text.Length;
            if (len <= length) return new MvcHtmlString(text);
            length = length > len ? len : length;
            string s = html.DisplayFor(expression).ToString().Substring(0, length) + "...";
            return new MvcHtmlString(s);
        }
    }
}