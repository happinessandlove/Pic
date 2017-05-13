using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageClassification.Enums
{
    public enum EnumCaptchaUsedFor
    {
        [Display(Name = "用户注册")]
        志愿者注册 = 1,

        [Display(Name = "用户密码重置")]
        志愿者密码重置 = 2,

        [Display(Name = "用户修改密码")]
        志愿者修改密码 = 3,
    }
}