/**************************************************
 * by 丁浩
 * 2016-01-30 
 * **************************************************/

using System.Configuration;

namespace System
{
    /// <summary>
    /// 获取Web.config文件中的appSettings
    /// </summary>
    public static partial class Config
    {
        /// <summary>
        /// 外键表联合查询时，AssociatedQuery标识是否加在外键成员上
        /// </summary>
        public static bool AssociatedQueryAttributeAddedOnForeignKey
        {
            get
            {
                var setting = ConfigurationManager.AppSettings["AssociatedQueryAttributeAddedOn"];
                if (setting == null) return false;
                else return setting.ToString() == "1";
            }
        }

        /// <summary>
        /// 列表页每页显示数据条数
        /// </summary>
        public static int PageSize
        {
            get
            {
                var setting = ConfigurationManager.AppSettings["PageSize"];
                if (setting == null) return 20;
                else return int.Parse(setting.ToString());
            }
        }

        /// <summary>
        /// 站点目录XML文件所放路径（相对路径）
        /// </summary>
        public static string SiteDirectoryPath
        {
            get
            {
                var setting = ConfigurationManager.AppSettings["SiteDirectoryPath"];
                if (setting == null) return "\\Directory\\SiteDirectory.xml";
                return setting.ToString();
            }
        }
    }
}
