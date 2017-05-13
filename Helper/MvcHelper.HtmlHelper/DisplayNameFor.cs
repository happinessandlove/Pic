/**************************************************
 * by 丁浩
 * 2016-02-01 
***************************************************/

using System.Collections.Generic;
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
        /// （自定义）显示该属性在Model类中设置的DisplayName值
        /// </summary>
        /// <param name="html"></param>
        /// <param name="expression">属性(lambda表达式)</param>
        /// <param name="finalMaxLength">插入全角空格后，字符串最大长度限制</param>
        /// <param name="addColon">是否在文字后面添加中文冒号，缺省值为false</param>
        /// <returns></returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int finalMaxLength, bool addColon = false)
        {
            return propertyDisplayNameHelper(html.DisplayNameFor(expression).ToString(), finalMaxLength, addColon);
        }

        /// <summary>
        /// （自定义）显示该属性在Model类中设置的Display值
        /// </summary>
        /// <param name="html"></param>
        /// <param name="expression">属性(lambda表达式)</param>
        /// <param name="finalMaxLength">插入全角空格后，字符串最大长度限制</param>
        /// <param name="addColon">是否在文字后面添加中文冒号，缺省值为false</param>
        /// <returns></returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> html, Expression<Func<TModel, TValue>> expression, int finalMaxLength, bool addColon = false)
        {
            return propertyDisplayNameHelper(html.DisplayNameFor(expression).ToString(), finalMaxLength, addColon);
        }

        private static MvcHtmlString propertyDisplayNameHelper(string displayName, int finalMaxLength, bool addColon)
        {
            int length = displayName.Length;
            if (length > 1 && length*2-1 <= finalMaxLength)
            {
                int spaceCount = 1;
                int currentLength = length * 2 - 1;
                while (currentLength < finalMaxLength)
                {
                    spaceCount++;
                    currentLength += length - 1;
                }
                if (currentLength > finalMaxLength) spaceCount--;
                if (spaceCount > 0)
                {
                    string space = "　";
                    for (int i = 1; i < spaceCount; i++)
                        space += "　";
                    displayName = string.Join(space, displayName.ToArray());
                }
            }
            if (addColon) displayName += "：";
            return new MvcHtmlString(displayName);
        }
    }
}