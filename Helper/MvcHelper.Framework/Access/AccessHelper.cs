/**************************************************
 * by 丁浩
 * 2016-01-11 
 * **************************************************/

using System.Collections.Generic;

namespace System.Web.Mvc
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Access
    {
        /// <summary>
        /// 访问权限字典，来源于Session。
        /// </summary>
        public static Dictionary<string, bool> All
        {
            get
            {
                return HttpContext.Current.Session["Access"] as Dictionary<string, bool>;
            }
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="directoryId">站点目录中id属性值</param>
        /// <returns></returns>
        public static bool Validate(string directoryId)
        {
            return (Access.All.ContainsKey(directoryId) && Access.All[directoryId]);
        }
    }
}