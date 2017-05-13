using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Helpers;
using Newtonsoft.Json;
using Models;

namespace MvcHelper.Management.Controllers
{
    public class RolesController : Controller
    {
        private DbEntity db = new DbEntity();
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #region 列表
        public ActionResult Index(OperationParam op)
        {
            #region 验证登录
            User loginUser = AccountHelper.LoginUser;
            if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
            ViewBag.LoginUser = loginUser;
            #endregion

            #region 验证权限
            string pageId = "Index_Role";//站点目录中的id属性值
            if (!Access.Validate(pageId)) return HttpNotFound();
            ViewBag.PageId = pageId;
            #endregion

            #region 删除
            ReturnValue returnValue = new ReturnValue();
            switch (op.OpType)
            {
                case OperationType.Pager: break;
                case OperationType.Query: break;
                case OperationType.Sort: break;
                case OperationType.ShowAll: break;
                case OperationType.RankUp: break;
                case OperationType.Delete:
                    try
                    {
                        Role role = db.Roles.Find(Guid.Parse(op.OpArgument));
                        if (role == null)
                        {
                            returnValue.Type = ReturnType.DeleteFailure;
                            returnValue.Message = ResultMessage.DeleteFailure;
                        }
                        else
                        {
                            db.Roles.Remove(role);
                            db.SaveChanges();
                            returnValue.Type = ReturnType.DeleteSuccess;
                            returnValue.Message = ResultMessage.DeleteSuccess;
                        }
                    }
                    catch
                    {
                        returnValue.Type = ReturnType.DeleteFailure;
                        returnValue.Message = ResultMessage.DeleteFailure;
                    }
                    OperationParam.Reset(ModelState);
                    break;
                case OperationType.Deletes:
                    try
                    {
                        string[] ids = op.OpArgument.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string id in ids)
                        {
                            Role role = db.Roles.Find(Guid.Parse(id));
                            if (role == null) continue;
                            else db.Roles.Remove(role);
                        }
                        db.SaveChanges();
                        returnValue.Type = ReturnType.DeletesSuccess;
                        returnValue.Message = ResultMessage.DeletesSuccess;
                    }
                    catch
                    {
                        returnValue.Type = ReturnType.DeletesFailure;
                        returnValue.Message = ResultMessage.DeletesFailure;
                    }
                    OperationParam.Reset(ModelState);
                    break;
            }
            #endregion

            IQueryable<Role> datas = QueryHelper.ExecuteQuery(db.Roles, op.OpQueryString);//前台查询条件（延迟执行）。若附加条件，添加Where
            Pager pager = new Pager(datas.Count(), op.OpPager, null);//页码相关对象
            ViewBag.Pager = pager;
            if (string.IsNullOrEmpty(op.OpSortProperty)) { op.OpSortProperty = "Name"; op.OpSortDirection = SortDirection.Descending; }//首次打开页面的初始排序依据及方向 <需修改>
            List<Role> roles = datas.Sort(op.OpSortProperty, op.OpSortDirection).GetPageData(pager).ToList();//数据排序、执行查询
            ViewBag.OperationParam = op;
            ViewBag.ReturnValue = returnValue.ToJson();
            return View(roles);
        }
        #endregion

        #region 详情
        //[HttpGet]
        //public ActionResult Details(Guid id)
        //{
        //    #region 验证登录
        //    User loginUser = AccountHelper.LoginUser;
        //    if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
        //    #endregion

        //    #region 验证权限
        //    string pageId = "Details_Role";//站点目录中的id属性值
        //    if (!Access.Validate(pageId)) return HttpNotFound();
        //    ViewBag.PageId = pageId;
        //    #endregion

        //    Role role = db.Roles.Find(id);
        //    if (role == null) return HttpNotFound();
        //    return View(role);
        //}
        #endregion

        #region 添加
        [HttpGet]
        public ActionResult Create()
        {
            #region 验证登录
            User loginUser = AccountHelper.LoginUser;
            if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
            #endregion

            #region 验证权限
            string pageId = "Create_Role";//站点目录中的id属性值
            if (!Access.Validate(pageId)) return HttpNotFound();
            ViewBag.PageId = pageId;
            #endregion

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role role)
        {
            #region 验证登录
            User loginUser = AccountHelper.LoginUser;
            if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
            #endregion

            #region 验证权限
            string pageId = "Create_Role";//站点目录中的id属性值
            if (!Access.Validate(pageId)) return HttpNotFound();
            ViewBag.PageId = pageId;
            #endregion

            #region 添加
            ReturnValue returnValue = new ReturnValue();
            if (ModelState.IsValid)
            {
                #region 特殊数据验证
                if (db.Roles.Where(s => s.Name == role.Name).Count() > 0)
                {
                    returnValue.Type = ReturnType.CreateFailure;
                    returnValue.Message = "该角色已存在";
                    ViewBag.ReturnValue = returnValue.ToJson();
                    return View(role);
                }
                #endregion
                try
                {
                    role.Id = Guid.NewGuid();
                    db.Roles.Add(role);
                    db.SaveChanges();
                    ModelState.Clear();
                    returnValue.Type = ReturnType.CreateSuccess;
                    returnValue.Message = ResultMessage.CreateSuccess;
                    ViewBag.ReturnValue = returnValue.ToJson();
                    return View();
                }
                catch
                {
                    returnValue.Type = ReturnType.CreateFailure;
                    returnValue.Message = ResultMessage.CreateFailure;
                    ViewBag.ReturnValue = returnValue.ToJson();
                    return View(role);
                }
            }
            #endregion

            ViewBag.ReturnValue = returnValue.ToJson();
            return View(role);
        }
        #endregion

        #region 编辑
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            #region 验证登录
            User loginUser = AccountHelper.LoginUser;
            if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
            #endregion

            #region 验证权限
            string pageId = "Edit_Role";//站点目录中的id属性值
            if (!Access.Validate(pageId)) return HttpNotFound();
            ViewBag.PageId = pageId;
            #endregion

            Role role = db.Roles.Find(id);
            if (role == null) return HttpNotFound();
            Dictionary<string, bool> access = JsonConvert.DeserializeObject<Dictionary<string, bool>>(role.MenuId);
            ViewBag.CurrentAccess = access;
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Role role)
        {
            #region 验证登录
            User loginUser = AccountHelper.LoginUser;
            if (loginUser == null) return RedirectToAction("Login", "Home", new { area = "" });
            #endregion

            #region 验证权限
            string pageId = "Edit_Role";//站点目录中的id属性值
            if (!Access.Validate(pageId)) return HttpNotFound();
            ViewBag.PageId = pageId;
            #endregion

            Dictionary<string, bool> access = JsonConvert.DeserializeObject<Dictionary<string, bool>>(role.MenuId);
            ViewBag.CurrentAccess = access;

            ReturnValue returnValue = new ReturnValue();
            #region 编辑
            if (ModelState.IsValid)
            {
                #region 特殊数据验证
                if (db.Roles.Where(s => s.Name == role.Name && s.Id != role.Id).Count() > 0)
                {
                    returnValue.Type = ReturnType.EditFailure;
                    returnValue.Message = "该角色已存在";
                    ViewBag.ReturnValue = returnValue.ToJson();
                    return View(role);
                }
                #endregion
                try
                {
                    db.Entry(role).State = EntityState.Modified;
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

            ViewBag.ReturnValue = returnValue.ToJson();
            return View(role);
        }
        #endregion

        #region ajax
        [HttpGet]
        public JsonResult RemoteCheck(string value)
        {
            ReturnValue returnValue = new ReturnValue();
            int count = db.Roles.Where(s => s.Name == value).Count();
            if (count == 0)
            {
                returnValue.Type = ReturnType.Success;
                returnValue.Message = ResultMessage.EditSuccess;
            }
            else
            {
                returnValue.Type = ReturnType.Failure;
                returnValue.Message = "该xxxx已存在";
            }
            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}