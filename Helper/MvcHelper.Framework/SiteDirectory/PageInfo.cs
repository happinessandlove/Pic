/**************************************************
 * by 丁浩
 * 2015-02-05 
 * **************************************************/

using System.Collections.Generic;

namespace System.Web.Mvc
{
    /// <summary>
    /// （自定义）网站页面封装类，数据来源于网站目录XML文件。
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 页面节点的类型
        /// </summary>
        public DirectoryType DirectoryType { get; set;}

        /// <summary>
        /// 页面节点的id，用在html页面中的元素中
        /// </summary>
        public string Id { get; set;}

        /// <summary>
        /// 页面节点name，对应Area、Controller或者Action的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 页面节点的namecn，用于权限管理时的显示名称
        /// </summary>
        public string NameCn { get; set;}

        /// <summary>
        /// 页面的url。只针对Action类型的节点。Controller类型的节点会自动生成一个缺省的Action
        /// </summary>
        public string Url { get; set;}

        /// <summary>
        /// 页面的标题，用于页面标签显示
        /// </summary>
        public string Title { get; set;}

        /// <summary>
        /// 页面对应数据库表所属的分类ID，即同一数据表分为两块管理。用于Controller中缺省的Action
        /// </summary>
        public string CategoryId { get; set;}

        /// <summary>
        /// 节点的深度
        /// </summary>
        public int Level { get; set;}

        /// <summary>
        /// 是否是缺省的页面
        /// </summary>
        public bool IsDefaultAction { get; set; }

        /// <summary>
        /// 父页面
        /// </summary>
        public PageInfo Parent { get; set; }
        
        /// <summary>
        /// 所有子页面
        /// </summary>
        public List<PageInfo> Children { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PageInfo()
        {
            this.IsDefaultAction = false;
            this.Children = new List<PageInfo>();
        }
    }
}