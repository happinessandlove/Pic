/**************************************************
 * by 丁浩
 * 2015-03-21 
 * **************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 菜单的Html辅助方法类。
    /// </summary>
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// 绘制菜单
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="siteDirectories">站点目录的封装集合</param>
        /// <param name="accessDictionary">所有访问权限字典</param>
        /// <returns></returns>
        public static MvcHtmlString RenderMenu<TModel>(this HtmlHelper<TModel> html, List<PageInfo> siteDirectories, Dictionary<string, bool> accessDictionary)
        {
            StringBuilder sb = new StringBuilder("<div id=\"menu-container\">");

            foreach (PageInfo area in siteDirectories.Where(s => s.DirectoryType == DirectoryType.Area))
            {
                if (accessDictionary.ContainsKey(area.Id) && accessDictionary[area.Id])
                {
                    sb.Append(string.Format("<a class=\"menu-group menu-level-{0}\" id=\"menu-group-{1}\" href=\"/\">{2}</a>", area.Level, area.Id, area.Title));
                    if (area.Children.Count > 0)
                    {
                        sb.Append("<div class=\"menu-items-container\">");
                        int i = 0;
                        foreach (PageInfo controller in area.Children)
                        {
                            PageInfo indexPage = controller.Children.First(s => s.IsDefaultAction);
                            if (accessDictionary.ContainsKey(indexPage.Id) && accessDictionary[indexPage.Id])
                            {
                                sb.Append(string.Format("<a class=\"menu-item menu-level-{0} {1}\" href=\"{2}\" target=\"_self\" id=\"menu-{4}\" tabid=\"{3}\" menuid=\"{4}\" tabtitle=\"{5}\">{5}</a>", controller.Level, i > 0 ? "" : "first-menu-item", indexPage.Url, indexPage.Id, controller.Id, controller.Title));
                                i++;
                            }
                        }
                        sb.Append("</div>");
                    }
                }
            }
            sb.Append("</div>");
            return new MvcHtmlString(sb.ToString());
        }
    }
}
