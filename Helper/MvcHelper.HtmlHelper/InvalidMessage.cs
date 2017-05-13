/**************************************************
 * by 丁浩
 * 2015-01-31 
***************************************************/

using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// HtmlHelper的扩展方法，用于呈现Html元素。
    /// </summary>
    public static partial class HtmlHelpers
    {
        ///// <summary>
        ///// （自定义）重新封装验证信息
        ///// </summary>
        ///// <param name="html"></param>
        ///// <param name="e">属性(lambda表达式)</param>
        ///// <returns></returns>
        //public static MvcHtmlString InvalidMessageFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> e)
        //{
        //    MvcHtmlString htmlElem = html.ValidationMessageFor(e);
        //    if (htmlElem != null)
        //    {
        //        string elem = html.ValidationMessageFor(e).ToHtmlString();
        //        Regex reg = new Regex(@"^<span(?<s>.*)</span>$");
        //        elem = reg.Match(elem).Groups["s"].Value;
        //        reg = new Regex(@".*?>(?<s>.*)");
        //        string content = reg.Match(elem).Groups["s"].Value;
        //        if (!string.IsNullOrEmpty(content)) elem = elem.Replace(content, "<td>" + content + "</td>");
        //        elem = "<table class=\"validation-tip\"><tr" + elem + "</tr></table>";
        //        return new MvcHtmlString(elem);
        //    }
        //    else return new MvcHtmlString("");
        //}
    }
}