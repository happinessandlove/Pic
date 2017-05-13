/**************************************************
 * by 丁浩
 * 2015-02-04
 * **************************************************/

using System.Text;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 生成页码的Html辅助方法类。
    /// </summary>
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// （自定义）绘制页码
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="pager">封装的页码信息</param>
        /// <returns></returns>
        public static MvcHtmlString RenderPager<TModel>(this HtmlHelper<TModel> html, Pager pager)
        {
            StringBuilder sb = new StringBuilder("<div class='pager-container'><div class='pager'>");
            string pagerEndHtml = "<div class='clear-float'></div></div><div class='clear-float'></div></div>";
            string dataCountHtml = "<span class='pager-item-count'>共<font class='pager-item-count-number'>{0}</font>条数据</span>";
            string pagerInfoHtml = "<span class='pager-page-count'>第<font class='pager-current-page-number'>{0}</font>页/共<font class='pager-page-count-number'>{1}</font>页</span>";
            string noPrevNextHtml = "<span class='pager-no-prev-next'>{0}</span><span class='pager-no-prev-next'>{1}</span>";
            string prevNextHtml = "<a href='/' class='pager-prev-next' page='{0}'>{1}</a>";
            string prevsNextsHtml = "<a href='/' class='pager-prevs-nexts' page='{0}'>...</a>";
            string currentPagerHtml = "<span class='pager-number-current'>{0}</span>";
            string pagerNumberHtml = "<a href='/' class='pager-number' page='{0}'>{0}</a>";
            string pagerSelectHtml = "<span class='pager-select-container'><select class='pager-select'>{0}</select></span>";
            string pagerSelectOptionHtml = "<option value='{0}' {1}>{0}</option>";
            StringBuilder sbSelect = new StringBuilder();

            //没有数据
            if (pager.DataCount == 0)
            {
                sb.AppendFormat(dataCountHtml, 0).Append(pagerEndHtml);
                return new MvcHtmlString(sb.ToString());
            }
            sb.AppendFormat(dataCountHtml, pager.DataCount).AppendFormat(pagerInfoHtml, pager.PageIndex, pager.PageCount);
            //一页数据
            if (pager.PageCount == 1)
            {
                sb.Append(pagerEndHtml);
                return new MvcHtmlString(sb.ToString());
            }

            int prev = (int)((pager.PagerNumberCount - 1) / 2);//4:1,5:2
            int next = pager.PagerNumberCount - prev - 1; //4:2,5:2
            int prevCount = prev, nextCount = next;
            int start;

            if (pager.PageIndex - prev < 1)
            {
                prevCount = pager.PageIndex - 1;
                nextCount = (pager.PagerNumberCount > pager.PageCount ? pager.PageCount : pager.PagerNumberCount) - prevCount - 1;
            }
            if (pager.PageIndex + next > pager.PageCount)
            {
                nextCount = pager.PageCount - pager.PageIndex;
                prevCount = (pager.PagerNumberCount > pager.PageCount ? pager.PageCount : pager.PagerNumberCount) - nextCount - 1;
            }
            if (pager.PageIndex == 1)
            {
                sb.AppendFormat(noPrevNextHtml, "首页", "上一页");
                start = 1;
            }
            else
            {
                sb.AppendFormat(prevNextHtml, 1, "首页").AppendFormat(prevNextHtml, pager.PageIndex - 1, "上一页");
                start = pager.PageIndex - prevCount;
                if (start > 1) sb.AppendFormat(prevsNextsHtml, (pager.PageIndex - prevCount - next - 1) > 0 ? (pager.PageIndex - prevCount - next - 1) : 1);
            }
            for (int i = 0; i < pager.PagerNumberCount && start <= pager.PageCount; i++, start++)
            {
                if (start == pager.PageIndex) sb.AppendFormat(currentPagerHtml, start);
                else sb.AppendFormat(pagerNumberHtml, start);
            }
            if (pager.PageIndex < pager.PageCount)
            {
                if (start <= pager.PageCount)
                {
                    sb.AppendFormat(prevsNextsHtml, (pager.PageIndex + nextCount + prev + 1) < pager.PageCount ? (pager.PageIndex + nextCount + prev + 1) : pager.PageCount);
                }
                sb.AppendFormat(prevNextHtml, pager.PageIndex + 1, "下一页");
                sb.AppendFormat(prevNextHtml, pager.PageCount, "末页");
            }
            else
            {
                sb.AppendFormat(noPrevNextHtml, "下一页", "末页");
            }

            for (int i = 1; i <= pager.PageCount; i++)
            {
                if (i == pager.PageIndex) sbSelect.AppendFormat(pagerSelectOptionHtml, i, "selected='selected'");
                else sbSelect.AppendFormat(pagerSelectOptionHtml, i, "");
            }
            sb.AppendFormat(pagerSelectHtml, sbSelect.ToString());
            sb.Append(pagerEndHtml);
            return new MvcHtmlString(sb.ToString());
        }
    }
}