/**************************************************
 * by 丁浩
 * 2016-01-31
 * **************************************************/

using System.Configuration;

namespace System.Web.Mvc
{
    /// <summary>
    /// （自定义）页码封装类
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// 数据记录总数
        /// </summary>
        public int DataCount { get; private set; }

        /// <summary>
        /// 每页显示记录数，缺省值在web.config中定义
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// 显示出的页码数目，超出时显示为...
        /// </summary>
        public int PagerNumberCount { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataCount">数据记录总数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数，为null时，取web.config中定义的缺省值</param>
        /// <param name="pagerNumberCount">显示出的页码数目，缺省值为8</param>
        public Pager(int dataCount, int pageIndex, int? pageSize, int pagerNumberCount = 8)
        {
            this.DataCount = dataCount;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize.HasValue ? pageSize.Value : Config.PageSize;
            this.PageCount = (int)Math.Ceiling(dataCount / (double)this.PageSize);
            this.PagerNumberCount = pagerNumberCount;

            if (dataCount == 0) this.PageIndex = 1;
            else if (this.PageIndex < 1) this.PageIndex = 1;
            else if (this.PageIndex > this.PageCount) this.PageIndex = this.PageCount;
        }
    }
}
