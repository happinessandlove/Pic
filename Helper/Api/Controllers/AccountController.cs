using ImageClassification.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace Api.Controllers
{
    public class AccountController : ApiController
    {
        private DbEntity db = new DbEntity();

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
        #region Post Login 登录
        [Route("Login")]
        public IHttpActionResult PostLogin(Login model)
        {
            //验证Android手机终端
            IEnumerable<string> headerUuid = null;
            ActionContext.Request.Headers.TryGetValues("uuid", out headerUuid);
            if (headerUuid == null)
                return BadRequest("请从Android手机操作");
            string uuid = null;
            if (headerUuid != null)
                uuid = headerUuid.ToList()[0];
            //验证数据
            if (model == null)
                return BadRequest("请填写登录信息");
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.First(s => s.Errors.Count > 0).Errors[0].ErrorMessage);
            Volunteer volunteer = db.Volunteers.FirstOrDefault(s => s.MobileNumber == model.MobileNumber && s.Status != EnumUserStatus.注销); //应为0个或1个
            //验证是否已注册
            if (volunteer == null)
                return BadRequest("该手机号码未注册");
            //验证账户是否锁定
            if (volunteer.Status == EnumUserStatus.密码输入错误临时锁定)
            {
                if (volunteer.LockoutEndTime < DateTime.Now)
                {
                    volunteer.Status = EnumUserStatus.正常;
                    volunteer.LockoutStartTime = null;
                    volunteer.LockoutDuration = 0;
                    db.SaveChanges();
                }
                else
                    return BadRequest("账户被锁定");
            }
            //验证密码
            if (volunteer.Password != SecurityHelper.MD5Hash(model.PassWord))
                return BadRequest("密码不正确");
            //更新uuid
            if (uuid != null) volunteer.Uuid = uuid;

            VolunteerToken volunteerToken = db.VolunteerTokens.FirstOrDefault(s => s.VolunteerId == volunteer.Id);
            DateTime now = DateTime.UtcNow;
            string token = SecurityHelper.MD5Hash(volunteer.Id.ToString() + now.ToString() + Guid.NewGuid().ToString());
            if (volunteerToken == null)
            {
                volunteerToken = new VolunteerToken
                {
                    VolunteerId = volunteer.Id,
                    CreateTime = now,
                    ActiveTime = now,
                    Token = token,
                    Validity = 30
                };
                try
                {
                    db.VolunteerTokens.Add(volunteerToken);
                    db.Entry(volunteerToken).State = EntityState.Added;
                    db.SaveChanges();
                }
                catch
                {
                    db.Entry(volunteerToken).State = EntityState.Unchanged;
                    return BadRequest("操作发生错误");
                }
            }
            else
            {
                volunteerToken.ActiveTime = now;
                volunteerToken.Token = token;
                volunteerToken.Validity = 30;
                db.Entry(volunteerToken).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    db.Entry(volunteerToken).State = EntityState.Unchanged;
                    return BadRequest("操作发生错误");
                }
            }
            return Ok(new
            {
                Token = token,
                Volunteer = VolunteerService.FormatUser(volunteer)
            });
        }
        #endregion

        #region Post Logout 登出
        [Route("Logout")]
        public IHttpActionResult PostLogout()
        {
            Guid volunteerId = VolunteerService.GetVolunteerIdFromIdentity();
            VolunteerToken volunteerToken = db.VolunteerTokens.FirstOrDefault(s => s.VolunteerId == volunteerId);
            try
            {
                volunteerToken.Validity = 0;
                db.Entry(volunteerToken).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {
                db.Entry(volunteerToken).State = EntityState.Unchanged;
                return BadRequest("操作发生错误");
            }
            return Ok();
        }
#endregion

        #region Post Register/SendCaptcha 获取手机注册验证码
        [Route("Register/SendCaptcha")]
        public IHttpActionResult PostSendRegisterCaptcha(GetCaptcha model)
        {
            //验证数据
            if(model==null)
                return BadRequest("请输入手机号码");
            if(!ModelState.IsValid)
                return BadRequest(ModelState.Values.First(s=>s.Errors.Count>0).Errors[0].ErrorMessage);

            Volunteer volunteer = db.Volunteers.FirstOrDefault(s=>s.MobileNumber==model.MobileNumber&&s.Status!=EnumUserStatus.注销);

            //生成验证码
            string code = new Random().Next(100000, 999999).ToString();
#if DEBUG
            code = "666666";
            Thread.Sleep(2000);
#endif
            //存储手机号码、验证码等信息到数据库
            //1.新手机号码
            if(volunteer==null)
            {
                DateTime now = DateTime.Now;
                volunteer = new Volunteer
                {
                    Id = Guid.NewGuid(),
                    MobileNumber = model.MobileNumber,
                    SmsCaptcha = code,
                    SmsCaptchaUsedFor = EnumCaptchaUsedFor.志愿者注册,
                    SmsCaptchaExpiredTime = now.AddMinutes(30),
                    RegisterTime = now
                };
                try
                {
                    db.Volunteers.Add(volunteer);
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    return BadRequest("操作发生错误");
                }

            }
            //2.已存在的手机号码
            else
            {
                //排除已经提交手机号码但未进行验证的情况
                if (volunteer.Status == EnumUserStatus.注册未验证手机)
                    return BadRequest("该手机号码已注册");
                volunteer.SmsCaptcha = code;
                volunteer.SmsCaptchaUsedFor = EnumCaptchaUsedFor.志愿者注册;
                DateTime now = DateTime.Now;
                volunteer.SmsCaptchaExpiredTime = now.AddMinutes(30);
                db.Entry(volunteer).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    db.Entry(volunteer).State = EntityState.Unchanged;
                    return BadRequest("操作发生错误");
                }

            }

#if !DEBUG
             if (!Ali.SendWorkerRegisterSms(code, model.MobileNumber))
            {
                //发送错误不处理
            }
#endif

            return Ok();
        }


        #endregion

        #region Post Register 提交用户注册信息
        [Route("Register")]
        public IHttpActionResult PostRegister(Register model)
        {
            DateTime now = DateTime.Now;
            //验证Andriod手机端
            IEnumerable<string> headUuid = null;
            ActionContext.Request.Headers.TryGetValues("uuid", out headUuid);
            string uuid = null;
            if (headUuid != null)
                uuid = headUuid.ToList()[0];
            //验证数据
            if (model == null)
                return BadRequest("请填写注册信息");
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.First(s => s.Errors.Count > 0).Errors[0].ErrorMessage);
            Volunteer volunteer = db.Volunteers.FirstOrDefault(s => s.MobileNumber == model.MobileNumber && s.Status != EnumUserStatus.注销);
            //未点击获取验证码
            if (volunteer == null)
                return BadRequest("请获取验证码");
            //手机号码已注册
            if (volunteer.Status == EnumUserStatus.注册未验证手机)
                return BadRequest("该手机号码已注册");
            //数据库中不存在验证码
            if (string.IsNullOrEmpty(volunteer.SmsCaptcha))
                return BadRequest("请获取验证码");
            //验证码用途不正确
            if (!volunteer.SmsCaptchaUsedFor.HasValue || volunteer.SmsCaptchaUsedFor != EnumCaptchaUsedFor.志愿者注册)
                return BadRequest("验证码无效，请重新获取验证码");
            //验证码失效
            if (volunteer.SmsCaptchaExpiredTime > now)
                return BadRequest("验证码失效，请重新获取");
            //验证码错误
            if (volunteer.SmsCaptcha != model.Captcha)
                return BadRequest("验证码错误");

            //注册，生成token
            volunteer.Uuid = uuid;
            volunteer.Password = SecurityHelper.MD5Hash(model.PassWord);
            volunteer.SmsCaptcha = null;
            volunteer.SmsCaptchaExpiredTime = null;
            volunteer.SmsCaptchaUsedFor = null;
            volunteer.Status = EnumUserStatus.注册中;
            volunteer.RegisterTime = now;
            db.Entry(volunteer).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
                VolunteerToken volunteerToken = db.VolunteerTokens.FirstOrDefault(s => s.VolunteerId == volunteer.Id);
                string token = SecurityHelper.MD5Hash(volunteer.Id.ToString() + now.ToString() + Guid.NewGuid().ToString());
                if (volunteerToken == null)
                {
                    volunteerToken = new VolunteerToken
                    {
                        VolunteerId = volunteer.Id,
                        Token = token,
                        CreateTime = now,
                        ActiveTime = now,
                        Validity = 30
                    };
                    db.VolunteerTokens.Add(volunteerToken);
                    db.SaveChanges();
                }
                else
                {
                    volunteerToken.ActiveTime = now;
                    volunteerToken.Token = token;
                    volunteerToken.Validity = 30;
                    db.Entry(volunteerToken).State = EntityState.Modified;
                    db.SaveChanges();
                }
            return Ok(
                new { Token = token , Volunteer=VolunteerService.FormatUser(volunteer) }
                );
        }
             catch (Exception ex)
            {
                db.Entry(volunteer).State = EntityState.Unchanged;
                return BadRequest("操作发生错误");
            }
        }
#endregion

    }
}
