/**************************************************
 * by 丁浩
 * 2015-01-31 
***************************************************/

using System.Linq;
using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// HtmlHelper的扩展方法，用于呈现Html元素。
    /// </summary>
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// （自定义）性别单选按钮组
        /// </summary>
        /// <param name="html"></param>
        /// <param name="e">属性(lambda表达式)</param>
        /// <returns></returns>
        public static MvcHtmlString GenderFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> e)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(e, html.ViewData);
            string propertyName = (metadata.PropertyName ?? ExpressionHelper.GetExpressionText(e).Split(new char[] { '.' }).Last<string>());
            string value = (metadata.Model == null ? null : metadata.Model.ToString());
            string s = html.RadioButtonFor(e, "男", (value == "男" ? new { @checked = "checked" } : null)).ToString() + "&nbsp;男&nbsp;&nbsp;&nbsp;" + html.RadioButtonFor(e, "女", (value == "女" ? new { @checked = "checked" } : null)).ToString() + "&nbsp;女";
            return new MvcHtmlString(s);
        }
    }
}