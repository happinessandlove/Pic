using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 交易中心用户登录表
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }

        #region 导航属性=====================================================================================
        [DisplayName("角色名称"),MyRequired,MySelect]
        public Guid RoleId { get; set; }
        [DisplayName("角色名称")]
        public virtual Role Role { get; set; }
        #endregion===========================================================================================

        [DisplayName("登录账号"), MyRequired, MyMaxLength(10)]
        public string LoginName { get; set; }

        [DisplayName("密码"),MyRequired, MyMaxLength(50),DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("姓名"),MyRequired, MyMaxLength(20)]
        public string Name { get; set; }

        [DisplayName("家庭住址"),MyMaxLength(200)]
        public string LivingAddress { get; set; }

        [DisplayName("联系电话"),MyMaxLength(20)]
        public string TelephoneNumber { get; set; }

        [DisplayName("手机"),MyMaxLength(20),MyPhone]
        public string MobileNumber { get; set; }

        [DisplayName("证件号码"),MyMaxLength(50)]
        public string IDCardNumber { get; set; }

        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }

        [DisplayName("员工简介"),MyMaxLength(1000)]
        public string Introduction { get; set; }

        [DisplayName("账户状态"),MySelect]
        public UserStatus Status { get; set; }

    }
}
