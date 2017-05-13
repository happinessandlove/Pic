using System;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using PhotoProcess;

namespace ManageWeb.Controllers
{
    public class HomeController : Controller
    {
        private DbEntity db = new DbEntity();
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index(string logintype = null)
        {
            //var d = SiteDirectory.Load();
            //string a = string.Join(",", d.Select(t => "\"" + t.Id + "\":true"));
            //a = "{" + a + "}";

            #region 验证登录
            User loginUser = AccountHelper.LoginUser;
            if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
            #endregion

            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUser loginUser)
        {
            if (ModelState.IsValid)
            {
                string pwd = SecurityHelper.MD5Hash(loginUser.Password);
                User user = db.Users.Include(s => s.Role).FirstOrDefault(t => t.LoginName == loginUser.UserName && t.Password == pwd);
                if (user != null)
                {
                    Session["LoginUser"] = user;
                    Dictionary<string, bool> access = JsonConvert.DeserializeObject<Dictionary<string, bool>>(user.Role.MenuId);
                    Session["access"] = access;
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("UserName", "用户名或密码不正确。");
            }
            return View(loginUser);
        }

        [HttpGet]
        public ActionResult LogOut()
        {

            Session["LoginUser"] = null;
            Session["access"] = null;
            return RedirectToAction("Login");
        }

        // 获取验证码
        [HttpGet]
        public ActionResult GetValidateCode()
        {
            byte[] bytes;
            Session["validateCode"] = ValidateCodeImage.Generate(out bytes, 4, 28, Color.FromArgb(221, 247, 255), Color.FromArgb(0, 183, 238), 16);
            return File(bytes, @"image/png");
        }
    }
}