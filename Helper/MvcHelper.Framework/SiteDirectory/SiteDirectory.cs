/**************************************************
 * by 丁浩
 * 2016-01-11 
 * **************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace System.Web.Mvc
{
    /// <summary>
    /// （自定义）封装站点目录，数据来源于网站目录XML文件。
    /// </summary>
    public class SiteDirectory
    {
        /// <summary>
        /// 封装站点目录
        /// </summary>
        /// <returns></returns>
        public static List<PageInfo> Load()
        {
            XElement root = XElement.Load(System.AppDomain.CurrentDomain.BaseDirectory + Config.SiteDirectoryPath);
            var nodes = root.Descendants().ToList();
            List<PageInfo> pageInfos = new List<PageInfo>();
            PageInfo area = null;
            PageInfo controller = null;
            PageInfo action = null;
            foreach (var node in nodes)
            {
                var titleAttr = node.Attribute("title");
                var idAttr = node.Attribute("id");
                var nameAttr = node.Attribute("name");
                var namecnAttr = node.Attribute("namecn");
                var actionAttr = node.Attribute("action");
                var defaultAttr = node.Attribute("defaultAction");
                var categoryIdAttr = node.Attribute("categoryid");

                DirectoryType directoryType = (DirectoryType)Enum.Parse(typeof(DirectoryType), node.Name.ToString());
                switch (directoryType)
                {
                    case DirectoryType.Area:
                        area = new PageInfo();
                        area.DirectoryType = directoryType;
                        area.Level = 1;
                        if (nameAttr == null) throw new Exception("Area类型的站点目录节点必须含有name属性！");
                        area.Name = nameAttr.Value;
                        if (idAttr == null) throw new Exception("Area类型的站点目录节点必须含有id属性！");
                        area.Id = idAttr.Value;
                        if (titleAttr == null) throw new Exception("Area类型的站点目录节点必须含有title属性！");
                        area.Title = titleAttr.Value;
                        if (namecnAttr == null) throw new Exception("Area类型的站点目录节点必须含有namecn属性！");
                        area.NameCn = namecnAttr.Value;
                        pageInfos.Add(area);
                        break;
                    case DirectoryType.Controller:
                        controller = new PageInfo();
                        controller.DirectoryType = directoryType;
                        controller.Level = 2;
                        if (nameAttr == null) throw new Exception("Controller类型的站点目录节点必须含有name属性！");
                        controller.Name = nameAttr.Value;
                        if (idAttr == null) throw new Exception("Controller类型的站点目录节点必须含有id属性！");
                        controller.Id = idAttr.Value;
                        if (titleAttr == null) throw new Exception("Controller类型的站点目录节点必须含有title属性！");
                        controller.Title = titleAttr.Value;
                        if (namecnAttr == null) throw new Exception("Controller类型的站点目录节点必须含有namecn属性！");
                        controller.NameCn = namecnAttr.Value;
                        area.Children.Add(controller);
                        controller.Parent = area;
                        pageInfos.Add(controller);
                        break;
                    case DirectoryType.Action:
                        action = new PageInfo();
                        action.DirectoryType = directoryType;
                        action.Level = 3;
                        if (nameAttr == null) throw new Exception("Action类型的站点目录节点必须含有name属性！");
                        action.Name = nameAttr.Value;
                        if (idAttr == null) throw new Exception("Action类型的站点目录节点必须含有id属性！");
                        action.Id = idAttr.Value;
                        if (titleAttr == null) throw new Exception("Action类型的站点目录节点必须含有title属性！");
                        action.Title = titleAttr.Value;
                        if (namecnAttr == null) throw new Exception("Action类型的站点目录节点必须含有namecn属性！");
                        action.NameCn = namecnAttr.Value;
                        action.CategoryId = categoryIdAttr == null ? null : categoryIdAttr.Value;
                        action.IsDefaultAction = defaultAttr == null ? false : (defaultAttr.Value == "true" ? true : false);
                        action.Url = string.Format("{0}/{1}/{2}{3}", (string.IsNullOrEmpty(area.Name) ? null : "/" + area.Name), controller.Name, action.Name, (string.IsNullOrEmpty(controller.CategoryId) ? null : "?categoryId=" + controller.CategoryId));
                        controller.Children.Add(action);
                        action.Parent = controller;
                        pageInfos.Add(action);
                        break;
                }
            }
            return pageInfos;
        }
        
        /// <summary>
        /// 获取所有网页信息，来源于Application对象。
        /// </summary>
        public static List<PageInfo> All
        {
            get
            {
                return HttpContext.Current.Application["SiteDirectories"] as List<PageInfo>;
            }
        }

        /// <summary>
        /// 根据网站目录节点id获取节点网页信息，来源于Application对象。
        /// </summary>
        /// <param name="directoryId">网站目录节点id</param>
        /// <returns></returns>
        public static PageInfo GetPageInfo(string directoryId)
        {
            return SiteDirectory.All.FirstOrDefault(s => s.Id == directoryId);
        }
    }
}