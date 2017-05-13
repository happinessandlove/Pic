/**************************************************
 * by 丁浩
 * 2016-01-31 
 * **************************************************/

using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// HtmlHelper的扩展方法，用于呈现Html元素。
    /// </summary>
    public static partial class HtmlHelpers
    {
        #region toolbar
        /// <summary>
        /// （自定义）绘制特殊工具栏按钮
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="pageId">站点目录Action节点的id</param>
        /// <param name="pageInfo">Action节点所属Controller页面信息</param>
        /// <param name="access">权限字典</param>
        /// <param name="jsFunction">点击后执行的js方法</param>
        /// <param name="urlParamString">url参数字符串</param>
        /// <param name="itemText">按钮显示文本，若为null，取站点目录中的title属性值</param>
        /// <param name="url">链接url，若为null，值为/{ControllerName}/{ActionName}</param>
        /// <param name="cssClass">样式名称，自动加前缀“tool-”,若为null，取Action节点的Name属性值</param>
        /// <param name="tabId">打开新标签页的id，若为null，取站点目录中的id属性值</param>
        /// <param name="tabTitle">打开新标签的显示文本，若为null，取站点目录中的title属性值</param>
        /// <param name="menuId">打开新标签对应的菜单，若为null，取pageInfo的id属性</param>
        /// <returns></returns>
        public static MvcHtmlString RenderCustomToolbarItem<TModel>(this HtmlHelper<TModel> html, string pageId, PageInfo pageInfo, Dictionary<string, bool> access, string jsFunction = null, string urlParamString = null, string itemText = null, string url = null, string cssClass = null, string tabId = null, string tabTitle = null, string menuId = null)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Id == pageId).FirstOrDefault();
            string item = null;
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
            {
                item = string.Format("<div class=\"tool-item-custom\"><a class=\"tool-custom tool-{0}\" href=\"{1}\" title=\"{2}\" tabid=\"{3}\" tabtitle=\"{2}\" menuid=\"{4}\" js=\"{5}\">{6}</a></div>"
                    , cssClass ?? page.Name, url ?? page.Url + (string.IsNullOrEmpty(urlParamString) ? null : "?" + urlParamString), tabTitle ?? page.Title, tabId ?? page.Id, menuId ?? pageInfo.Id, jsFunction, itemText ?? page.Title);
            }
            return new MvcHtmlString(item);
        }

        /// <summary>
        /// （自定义）绘制刷新工具栏按钮
        /// </summary>
        /// <param name="html"></param>
        public static MvcHtmlString RenderRefreshToolbarItem<TModel>(this HtmlHelper<TModel> html)
        {
            return new MvcHtmlString("<div class=\"tool-item\"><a class=\"tool-refresh\" href=\"#\" title=\"刷新页面\"></a></div>");
        }

        /// <summary>
        /// （自定义）绘制创建工具栏按钮
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageInfo">Controller页面信息</param>
        /// <param name="access">权限字典</param>
        public static MvcHtmlString RenderCreateToolbarItem<TModel>(this HtmlHelper<TModel> html, PageInfo pageInfo, Dictionary<string, bool> access)
        {
            string htmlString = null;
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Create").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
            {
                htmlString = string.Format("<div class=\"tool-item\"><a class=\"tool-create\" href=\"{0}\" tabid=\"{1}\" title=\"{2}\" tabtitle=\"{2}\" menuid=\"{3}\"></a></div>", page.Url, page.Id, page.Title, pageInfo.Id);
            }
            return new MvcHtmlString(htmlString);
        }

        /// <summary>
        /// （自定义）绘制删除工具栏按钮
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageInfo">Controller页面信息</param>
        /// <param name="access">权限字典</param>
        public static MvcHtmlString RenderDeleteToolbarItem<TModel>(this HtmlHelper<TModel> html, PageInfo pageInfo, Dictionary<string, bool> access)
        {
            string htmlString = null;
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Delete").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
            {
                htmlString = "<div class=\"tool-item\"><a class=\"tool-deletes\" href=\"/\" title=\"删除选中项\"></a></div>";
            }
            return new MvcHtmlString(htmlString);
        }

        /// <summary>
        /// （自定义）绘制查询工具栏按钮
        /// </summary>
        /// <param name="html"></param>
        public static MvcHtmlString RenderQueryToolbarItem<TModel>(this HtmlHelper<TModel> html)
        {
            return new MvcHtmlString("<div class=\"tool-item\"><a class=\"tool-query\" href=\"/\" title=\"查询\"></a></div><div class=\"tool-item\"><a class=\"tool-showAll\" href=\"/\" title=\"显示全部数据\"></a></div>");
        }

        /// <summary>
        /// （自定义）绘制导出工具栏按钮
        /// </summary>
        /// <param name="html"></param>
        /// <param name="currentPageInfo">页面信息</param>
        /// <param name="access">权限字典</param>
        /// <param name="id">导出项的主键</param>
        public static MvcHtmlString RenderExportToolbarItem<TModel>(this HtmlHelper<TModel> html, PageInfo currentPageInfo, Dictionary<string, bool> access, object id)
        {
            string htmlString = null;
            PageInfo page = currentPageInfo.Parent.Children.Where(s => s.Name == "Export").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
            {
                htmlString = string.Format("<div class=\"tool-item\"><a class=\"tool-export\" href=\"{0}?id={1}\" title=\"导出\"></a></div>", page.Url, id.ToString());
            }
            return new MvcHtmlString(htmlString);
        }

        /// <summary>
        /// （自定义）绘制返回工具栏按钮，返回到Default Action页面
        /// </summary>
        /// <param name="html"></param>
        /// <param name="currentPageInfo">页面信息</param>
        public static MvcHtmlString RenderReturnToolbarItem<TModel>(this HtmlHelper<TModel> html, PageInfo currentPageInfo)
        {
            PageInfo defaultPage = currentPageInfo.Parent.Children.FirstOrDefault(s => s.IsDefaultAction);
            return new MvcHtmlString(string.Format("<div class=\"tool-item\"><a class=\"tool-return\" href=\"{0}\" tabid=\"{1}\" title=\"返回{2}页\" tabtitle=\"{2}\" menuid=\"{3}\"></a></div>", defaultPage.Url, defaultPage.Id, currentPageInfo.Parent.Title, currentPageInfo.Parent.Id));
        }

        /// <summary>
        /// （自定义）绘制返回工具栏按钮，返回到指定页面
        /// </summary>
        /// <param name="html"></param>
        /// <param name="currentPageInfo">页面信息</param>
        /// <param name="returnPageId">返回的页面id</param>
        /// <param name="tabId">标签的Id，缺省值为页面节点的id</param>
        public static MvcHtmlString RenderReturnToolbarItem<TModel>(this HtmlHelper<TModel> html, PageInfo currentPageInfo, string returnPageId, string tabId = null)
        {
            PageInfo page = currentPageInfo.Parent.Children.FirstOrDefault(s => s.Id == returnPageId);
            return new MvcHtmlString(string.Format("<div class=\"tool-item\"><a class=\"tool-return\" href=\"{0}\" tabid=\"{1}\" title=\"返回{2}页\" tabtitle=\"{2}\" menuid=\"{3}\"></a></div>", page.Url, tabId ?? page.Id, page.Title, page.Parent.Id));
        }

        #endregion

        #region ColumnHeader
        /// <summary>
        /// （自定义）生成列标题
        /// </summary>
        /// <typeparam name="TModel">页面模型类型</typeparam>
        /// <typeparam name="TDisplayProperty">显示属性的类型</typeparam>
        /// <param name="html"></param>
        /// <param name="displayPropertyExpression">显示属性的lambda表达式</param>
        /// <param name="finalMaxLength">在获取到的DisplayName值的文字与文字中插入全角空格后，字符串最大长度限制，缺省值为4</param>
        /// <returns></returns>
        public static MvcHtmlString ColumnHeaderFor<TModel, TDisplayProperty>(this HtmlHelper<IEnumerable<TModel>> html, Expression<Func<TModel, TDisplayProperty>> displayPropertyExpression, int finalMaxLength = 3)
        {
            string displayPropertyName = ExpressionHelper.GetExpressionText(displayPropertyExpression);
            string th = string.Format("<th class=\"col-{0}\"><span>{1}</span></th>", displayPropertyName, html.DisplayNameFor(displayPropertyExpression, finalMaxLength, false));
            return new MvcHtmlString(th);
        }

        /// <summary>
        /// （自定义）生成带排序功能的列标题
        /// </summary>
        /// <typeparam name="TModel">页面模型类型</typeparam>
        /// <typeparam name="TProperty">显示属性以及排序属性的类型</typeparam>
        /// <param name="html"></param>
        /// <param name="propertyExpression">显示属性以及排序属性的lambda表达式，显示和排序属性相同</param>
        /// <param name="finalMaxLength">在获取到的DisplayName值的文字与文字中插入全角空格后，字符串最大长度限制，缺省值为4</param>
        /// <returns></returns>
        public static MvcHtmlString SortColumnHeaderFor<TModel, TProperty>(this HtmlHelper<IEnumerable<TModel>> html, Expression<Func<TModel, TProperty>> propertyExpression, int finalMaxLength = 3)
        {
            string propertyName = ExpressionHelper.GetExpressionText(propertyExpression);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<th class=\"col-{0}\"><a href=\"/\" class=\"sort\" property=\"{0}\">", propertyName);
            sb.Append(html.DisplayNameFor(propertyExpression, finalMaxLength, false));
            sb.Append("</a></th>");
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// （自定义）生成带排序功能的列标题，针对外键导航属性
        /// </summary>
        /// <typeparam name="TModel">页面模型类型</typeparam>
        /// <typeparam name="TDisplayProperty">显示属性的类型</typeparam>
        /// <typeparam name="TSortProperty">排序属性的类型</typeparam>
        /// <param name="html"></param>
        /// <param name="displayPropertyExpression">显示属性的lambda表达式 例：model=>model.Class</param>
        /// <param name="sortPropertyExpression">排序属性的lambda表达式 例：model=>model.Class.Name</param>
        /// <param name="finalMaxLength">在获取到的DisplayName值的文字与文字中插入全角空格后，字符串最大长度限制，缺省值为4</param>
        /// <returns></returns>
        public static MvcHtmlString SortColumnHeaderFor<TModel, TDisplayProperty, TSortProperty>(this HtmlHelper<IEnumerable<TModel>> html, Expression<Func<TModel, TDisplayProperty>> displayPropertyExpression, Expression<Func<TModel, TSortProperty>> sortPropertyExpression, int finalMaxLength = 3)
        {
            //string displayPropertyName = ExpressionHelper.GetExpressionText(displayPropertyExpression);
            string sortPropertyName = ExpressionHelper.GetExpressionText(sortPropertyExpression);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<th class=\"col-{0}\"><a href=\"/\" class=\"sort\" property=\"{1}\">", sortPropertyName.Replace(".",""), sortPropertyName);
            sb.Append(html.DisplayNameFor(displayPropertyExpression, finalMaxLength, false));
            sb.Append("</a></th>");
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// （自定义）绘制列表页标题栏的“序号”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderNoColumnHeader<TModel>(this HtmlHelper<TModel> html)
        {
            return new MvcHtmlString("<th class=\"col-no\"><span>序号</span></th>");
        }

        /// <summary>
        /// （自定义）绘制列表页标题栏的“全选”复选框
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">Controller页面信息</param>
        /// <returns></returns>
        public static MvcHtmlString RenderSelectAllColumnHeader<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Delete").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString("<th class=\"col-select\"><span><input type=\"checkbox\" id=\"select-all\" name=\"select-all\" /></span></th>");
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制列表页标题栏的“详情”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">Controller页面信息</param>
        /// <returns></returns>
        public static MvcHtmlString RenderDetailsColumnHeader<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Details").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString("<th class=\"col-details\"><span>详情</span></th>");
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制列表页标题栏的“排序”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">Controller页面信息</param>
        /// <returns></returns>
        public static MvcHtmlString RenderRankUpColumnHeader<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "RankUp").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString("<th class=\"col-rank\"><span>排序</span></th>");
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制列表页标题栏的“编辑”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">Controller页面信息</param>
        /// <param name="title">列标题名称</param>
        /// <returns></returns>
        public static MvcHtmlString RenderEditColumnHeader<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo, string title = null)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Edit").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString(string.Format("<th class=\"col-edit\"><span>{0}</span></th>", title ?? "编辑"));
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制列表页标题栏的“删除”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">Controller页面信息</param>
        /// <returns></returns>
        public static MvcHtmlString RenderDeleteColumnHeader<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Delete").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString("<th class=\"col-delete\"><span>删除</span></th>");
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制自定义的列表页标题栏
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="pageId">站点目录Action节点的id</param>
        /// <param name="access">权限字典</param>
        /// <param name="controllerPageInfo">Controller页面信息</param>
        /// <param name="cssClass">样式名称，自动加前缀“col-”,若为null，取Action节点的name属性值</param>
        /// <param name="headerText">列标题，若为null，取Action节点的namecn属性值</param>
        /// <returns></returns>
        public static MvcHtmlString RenderCustomColumnHeader<TModel>(this HtmlHelper<TModel> html, string pageId, Dictionary<string, bool> access, PageInfo controllerPageInfo, string cssClass = null, string headerText = null)
        {
            PageInfo page = controllerPageInfo.Children.Where(s => s.Id == pageId).FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString(string.Format("<th class=\"col-{0}\"><span>{1}</span></th>", cssClass ?? page.Name, headerText ?? page.NameCn));
            else return new MvcHtmlString("");
        }
        #endregion

        #region column
        /// <summary>
        /// （自定义）绘制列表页数据项的“序号”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="index">当前数据项在本页数据中的次序</param>
        /// <param name="pager">封装的页码信息</param>
        /// <returns></returns>
        public static MvcHtmlString RenderNoColumn<TModel>(this HtmlHelper<TModel> html, int index, Pager pager)
        {
            return new MvcHtmlString(string.Format("<td class=\"col-no\">{0}</td>", (pager.PageIndex - 1) * pager.PageSize + index + 1));
        }

        /// <summary>
        /// 绘制列表页数据项的“选中”复选框
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">站点目录封装的页面信息</param>
        /// <param name="itemId">当前数据项的主键值</param>
        /// <returns></returns>
        public static MvcHtmlString RenderSelectColumn<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo, object itemId)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Delete").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString(string.Format("<td class=\"col-select\"><input class=\"select-item\" type=\"checkbox\" value=\"{0}\" /></td>", itemId.ToString()));
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// 绘制空的列表页数据项的“选中”复选框
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">站点目录封装的页面信息</param>
        /// <returns></returns>
        public static MvcHtmlString RenderEmptySelectColumn<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Delete").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString("<td class=\"col-select\"></td>");
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制列表页数据项的“排序”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">站点目录封装的页面信息</param>
        /// <param name="itemId">当前数据项的主键值</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="index">当前数据项在本页数据中的次序</param>
        /// <returns></returns>
        public static MvcHtmlString RenderRankUpColumn<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo, object itemId, int pageIndex, int index)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "RankUp").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
            {
                if (pageIndex == 1 && index == 0)
                    return new MvcHtmlString("<td class=\"col-rank\"></td>");
                else
                    return new MvcHtmlString(string.Format("<td class=\"col-rank\"><a href=\"{0}\" class=\"button-rank\"></a></td>", itemId.ToString()));
            }
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制空的列表页数据项的“排序”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">站点目录封装的页面信息</param>
        /// <returns></returns>
        public static MvcHtmlString RenderEmptyRankUpColumn<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "RankUp").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
            {
                return new MvcHtmlString("<td class=\"col-rank\"></td>");
            }
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制列表页数据项的“详情”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">站点目录封装的页面信息</param>
        /// <param name="itemId">当前数据项的主键值</param>
        /// <param name="tabTitle">当前项的主要名称，显示在页面标签中</param>
        /// <returns></returns>
        public static MvcHtmlString RenderDetailsColumn<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo, object itemId, string tabTitle = null)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Details").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString(string.Format("<td class=\"col-details\"><a href=\"{0}id={1}\" class=\"button-details\" tabid=\"{2}\" title=\"查看详情\" tabtitle=\"{3}\" menuid=\"{4}\"></a></td>", (page.Url.Contains("?") ? page.Url + "&" : page.Url + "?"), itemId.ToString(), page.Id + itemId.ToString(), page.Title + (tabTitle == null ? "" : ("-" + tabTitle)), pageInfo.Id));
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制空的列表页数据项的“详情”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">站点目录封装的页面信息</param>
        /// <returns></returns>
        public static MvcHtmlString RenderEmptyDetailsColumn<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Details").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString("<td class=\"col-details\"></td>");
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制列表页数据项的“编辑”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">站点目录封装的页面信息</param>
        /// <param name="itemId">当前数据项的主键值</param>
        /// <param name="tabTitle">当前项的主要名称，显示在页面标签中</param>
        /// <returns></returns>
        public static MvcHtmlString RenderEditColumn<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo, object itemId, string tabTitle = null)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Edit").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
            {
                return new MvcHtmlString(string.Format("<td class=\"col-edit\"><a href=\"{0}id={1}\" class=\"button-edit\" tabid=\"{2}\" title=\"编辑\" tabtitle=\"{3}\" menuid=\"{4}\"></a></td>", (page.Url.Contains("?") ? page.Url + "&" : page.Url + "?"), itemId.ToString(), page.Id + itemId.ToString(), page.Title + (tabTitle == null ? "" : ("-" + tabTitle)), pageInfo.Id));
            }
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制空的列表页数据项的“编辑”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">站点目录封装的页面信息</param>
        /// <returns></returns>
        public static MvcHtmlString RenderEmptyEditColumn<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Edit").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
            {
                return new MvcHtmlString("<td class=\"col-edit\"></td>");
            }
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制列表页数据项的“删除”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">站点目录封装的页面信息</param>
        /// <param name="itemId">当前数据项的主键值</param>
        /// <returns></returns>
        public static MvcHtmlString RenderDeleteColumn<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo, object itemId)
        {
            PageInfo defaultPage = pageInfo.Children.FirstOrDefault(s => s.IsDefaultAction);
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Delete").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString(string.Format("<td class=\"col-delete\"><a href=\"/\" itemid=\"{0}\" class=\"button-delete\" tabid=\"{1}\" title=\"删除\"></a></td>", itemId.ToString(), defaultPage.Id));
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制列表页数据项的“删除”
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">站点目录封装的页面信息</param>
        /// <returns></returns>
        public static MvcHtmlString RenderEmptyDeleteColumn<TModel>(this HtmlHelper<TModel> html, Dictionary<string, bool> access, PageInfo pageInfo)
        {
            PageInfo defaultPage = pageInfo.Children.FirstOrDefault(s => s.IsDefaultAction);
            PageInfo page = pageInfo.Children.Where(s => s.Name == "Delete").FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString("<td class=\"col-delete\"></td>");
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制自定义的列表页数据项的操作按钮
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="pageId">站点目录Action节点的id</param>
        /// <param name="access">权限字典</param>
        /// <param name="controllerPageInfo">Action节点所属Controller页面信息</param>
        /// <param name="itemId">当前数据项的主键值</param>
        /// <param name="tabTitle">打开新标签的显示文本，若为null，取站点目录中的title属性值，否则为“站点目录中的title属性值-tabTitle”</param>
        /// <param name="jsFunction">点击后执行的js方法</param>
        /// <param name="itemText">按钮显示文本，若为null，取站点目录中的title属性值</param>
        /// <param name="url">链接url，若为null，值为/{ControllerName}/{ActionName}</param>
        /// <param name="urlParamString">url参数字符串</param>
        /// <param name="cssClass">样式名称，自动加前缀“button-”,若为null，取Action节点的Name属性值</param>
        /// <param name="tabId">打开新标签页的id，若为null，取站点目录中的id属性值</param>
        /// <param name="menuId">打开新标签对应的菜单，若为null，取pageInfo的id属性</param>
        /// <param name="htmlAttributes">其他html标签的属性</param>
        /// <returns></returns>
        public static MvcHtmlString RenderCustomColumn<TModel>(this HtmlHelper<TModel> html, string pageId, Dictionary<string, bool> access, PageInfo controllerPageInfo, object itemId, string tabTitle = null, string jsFunction = null, string itemText = null, string url = null, string urlParamString = null, string cssClass = null, string tabId = null, string menuId = null, string htmlAttributes = null)
        {
            PageInfo page = controllerPageInfo.Children.Where(s => s.Id == pageId).FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString(string.Format("<td class=\"col-custom col-{0}\"><a class=\"button-custom button-{0}\" href=\"{1}\" itemid=\"{2}\" title=\"{3}\" tabid=\"{4}\" tabtitle=\"{3}\" menuid=\"{5}\" {6} {8}>{7}</a></td>"
                    , cssClass ?? page.Name, url ?? page.Url + (itemId == null ? null : "?id=" + itemId.ToString()), itemId.ToString(), tabTitle == null ? page.Title : tabTitle, (tabId ?? page.Id) + (itemId == null ? null : itemId.ToString()), menuId ?? controllerPageInfo.Id, jsFunction == null ? null : "js=\"" + jsFunction + "\"", itemText ?? page.Title, htmlAttributes));
            else return new MvcHtmlString("");
        }

        /// <summary>
        /// （自定义）绘制空的列表页数据项的列
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="pageId">站点目录Action节点的id</param>
        /// <param name="access">权限字典</param>
        /// <param name="pageInfo">Action节点所属Controller页面信息</param>
        /// <param name="cssClass">样式名称，自动加前缀“col-”,若为null，取Action节点的Name属性值</param>
        /// <returns></returns>
        public static MvcHtmlString RenderEmptyCustomColumn<TModel>(this HtmlHelper<TModel> html, string pageId, Dictionary<string, bool> access, PageInfo pageInfo, string cssClass = null)
        {
            PageInfo page = pageInfo.Children.Where(s => s.Id == pageId).FirstOrDefault();
            if (page != null && access.ContainsKey(page.Id) && access[page.Id])
                return new MvcHtmlString(string.Format("<td class=\"col-custom col-{0}\"></td>", cssClass));
            else return new MvcHtmlString("");
        }
        #endregion
    }
}