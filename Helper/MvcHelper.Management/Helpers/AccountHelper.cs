using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace System.Web.Mvc
{
    public class AccountHelper
    {
        /// <summary>
        /// 获取Session中存储的用户对象。
        /// <para>仅将Session中的对象取出，不重新查询数据库。</para>
        /// </summary>
        public static User LoginUser
        {
            get
            {
                //开发时
                if (HttpContext.Current.Session["LoginUser"] == null)
                {
                    DbEntity db = new DbEntity();
                    User user = db.Users.Include(s => s.Role).First(t => t.LoginName == "admin");
                    if (user != null)
                    {
                        HttpContext.Current.Session["LoginUser"] = user;
                        Dictionary<string, bool> access = JsonConvert.DeserializeObject<Dictionary<string, bool>>(user.Role.MenuId);
                        HttpContext.Current.Session["Access"] = access;
                    }
                }

                if (HttpContext.Current.Session["LoginUser"] == null) return null;
                return HttpContext.Current.Session["LoginUser"] as User;
            }
        }
    }
}