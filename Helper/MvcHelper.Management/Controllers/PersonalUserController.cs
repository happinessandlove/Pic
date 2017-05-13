using Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MvcHelper.Management.Controllers
{
    public class PersonalUserController : Controller
    {
        private DbEntity db = new DbEntity();
    
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #region 详情
        [HttpGet]
        public ActionResult Details()
        {
            #region 验证登录
            User loginUser = AccountHelper.LoginUser;
            if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
            #endregion

            #region 验证权限
            string pageId = "Details_PersonalUser";//站点目录中的id属性值
            if (!Access.Validate(pageId)) return HttpNotFound();
            ViewBag.PageId = pageId;
            #endregion
            User user =db.Users.Find( loginUser.Id);
            if (user == null) return HttpNotFound();
            return View(user);
        }
        #endregion

        #region 个人信息维护
        [HttpGet]
        public ActionResult Edit()
        {
            #region 验证登录
            User loginUser = AccountHelper.LoginUser;
            if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
            #endregion

            #region 验证权限
            string pageId = "Edit_PersonalUser";//站点目录中的id属性值
            if (!Access.Validate(pageId)) return HttpNotFound();
            ViewBag.PageId = pageId;
            #endregion
            
            User user = db.Users.Include(s => s.Role).FirstOrDefault(s => s.Id == loginUser.Id); //Include所有需要的导航属性和导航集合（含多级导航），一次性查询数据库 <需修改>
            if (user == null) return HttpNotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            #region 验证登录
            User loginUser = AccountHelper.LoginUser;
            if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
            #endregion

            #region 验证权限
            string pageId = "Edit_PersonalUser";//站点目录中的id属性值
            if (!Access.Validate(pageId)) return HttpNotFound();
            ViewBag.PageId = pageId;
            #endregion

            ReturnValue returnValue = new ReturnValue();
            #region 编辑
            if (ModelState.IsValid)
            {
                #region 特殊数据验证
                if (db.Users.Where(s => s.LoginName == user.LoginName && s.Id != user.Id).Count() > 0)
                {
                    returnValue.Type = ReturnType.EditFailure;
                    returnValue.Message = "该登录账号已存在";
                    user = db.Users.Include(s => s.Role).FirstOrDefault(s => s.Id == user.Id);
                    ViewBag.ReturnValue = returnValue.ToJson();
                    return View(user);
                }
                #endregion
                try
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    returnValue.Type = ReturnType.EditSuccess;
                    returnValue.Message = ResultMessage.EditSuccess;
                }
                catch
                {
                    returnValue.Type = ReturnType.EditFailure;
                    returnValue.Message = ResultMessage.EditFailure;
                }
            }
            #endregion

            user = db.Users.Include(s => s.Role).FirstOrDefault(s => s.Id == user.Id);
            ViewBag.ReturnValue = returnValue.ToJson();
            return View(user);
        }
        #endregion

        #region 登录密码修改
        [HttpGet]
        public ActionResult ModifyPwd()
        {
            #region 验证登录
            User loginUser = AccountHelper.LoginUser;
            if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
            ViewBag.LoginUser = loginUser;
            #endregion

            #region 验证权限
            string pageId = "ModifyPwd_PersonalUser";//站点目录中的id属性值
            if (!Access.Validate(pageId)) return HttpNotFound();
            ViewBag.PageId = pageId;
            #endregion
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyPwd(ChangePassowrd model)
        {
            #region 验证登录
            User loginUser = AccountHelper.LoginUser;
            if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
            #endregion

            #region 验证权限
            string pageId = "ModifyPwd_PersonalUser";//站点目录中的id属性值
            if (!Access.Validate(pageId)) return HttpNotFound();
            ViewBag.PageId = pageId;
            #endregion

            User user = db.Users.Find(loginUser.Id);
            ViewBag.LoginUser = user;

            ReturnValue returnValue = new ReturnValue();
            if (ModelState.IsValid)
            {
                try
                {
                    string pwd = SecurityHelper.MD5Hash(model.Password);
                    if (pwd != user.Password)
                    {
                        ModelState.AddModelError("Password", "原密码不正确");
                        return View(model);
                    }
                    user.Password = SecurityHelper.MD5Hash(model.NewPassword);
                    db.SaveChanges();
                    returnValue.Type = ReturnType.EditSuccess;
                    returnValue.Message = "修改密码成功！";
                    model.Password = null;
                    model.NewPassword = null;
                    model.NewPasswordConfirm = null;
                    ModelState.Clear();
                }
                catch
                {
                    returnValue.Type = ReturnType.EditFailure;
                    returnValue.Message = "修改密码失败，请重新尝试！";
                    ModelState.Clear();
                }
            }

            ViewBag.ReturnValue = returnValue.ToJson();
            return View(model);
        }
        #endregion
        
    }
}