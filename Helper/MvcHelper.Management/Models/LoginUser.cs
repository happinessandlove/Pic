using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class LoginUser
    {
        [DisplayName("登录账号"), MyRequired, MyMaxLength(10)]
        public string UserName { get; set; }
        [DisplayName("密码"), MyRequired, MyMaxLength(10), DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("验证码"), MyRequired, MyFixedLength(4)]
        public string ValidateCode { get; set; }
    }
    public class ChangePassowrd
    {
        [DisplayName("原密码"), MyRequired, MyMaxLength(10), DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("新密码"), MyRequired, MyMaxLength(10), DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DisplayName("重复新密码"), MyRequired, MyMaxLength(10), MyCompare("NewPassword", ValidationCompareOperator.Equal, ValidationDataType.String, ErrorMessage = "两次输入的密码不同"), DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }
    }
}