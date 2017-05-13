using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageClassification.Enums
{
    public enum EnumUserStatus
    {
        注册未验证手机=1,
        注册中=2,
        待审核=3,
        审核未通过=4,
        正常=5,
        密码输入错误临时锁定=6,
        注销=7
    }
}