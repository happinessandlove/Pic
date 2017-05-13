using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    /// <summary>
    /// 获取注册手机验证码模型类
    /// </summary>
    public class GetCaptcha
    {
        [DisplayName("手机号码")]
        [MyRequired,MyMobile]
        public string MobileNumber { get; set; }
    }
    /// <summary>
    /// 登录模型类
    /// </summary>
    public class Login
    {
        [DisplayName("手机号码")]
        [MyRequired,MyMobile]
        public string MobileNumber { get; set; }

        [DisplayName("账户密码")]
        public string PassWord { get; set; }
    }

    public class Register
    {
        [DisplayName("手机号码")]
        [MyRequired,MyMobile]
        public string MobileNumber { get; set; }

        [DisplayName("账户密码")]
        public string PassWord { get; set; }

        [DisplayName("验证码")]
        [MyRequired,MyMaxLength(6)]
        public string Captcha { get; set; }
    }
}