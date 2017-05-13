using ImageClassification.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 交易中心用户登录表
    /// </summary>
    public class Volunteer
    {
        public Volunteer()
        {
           // this.Pictures = new List<Picture>();
            this.Types = new List<Type>();
            this.Labels = new List<Label>();
        }
        public Guid Id { get; set; }

        [MyMaxLength(100), NonQuery]
        public string Uuid { get; set; }

        #region 导航属性=====================================================================================
       // public virtual List<Picture> Pictures { get; set; }
        public virtual List<Type> Types { get; set; }
        public virtual List<Label> Labels { get; set; }

        /*public Guid CareerId { get; set; }
        public virtual Career Career { get; set; }*/
        #endregion===========================================================================================

        [DisplayName("登录账号"), MyMaxLength(10)]
        public string LoginName { get; set; }


        #region 安全相关
        [DisplayName("密码"), NonQuery, MyMaxLength(50),DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("短信验证码"), MyMaxLength(10), MyNumber, NonQuery]
        public string SmsCaptcha { get; set; }

        [DisplayName("短信验证码用途"), NonQuery]
        public EnumCaptchaUsedFor? SmsCaptchaUsedFor { get; set; }

        [DisplayName("短信验证码过期时间"), NonQuery]
        public DateTime? SmsCaptchaExpiredTime { get; set; }

        [DisplayName("登录失败次数")]
        public int? AccessFailedCount { get; set; }

        [DisplayName("账户锁定开始时间"), MyDate]
        public DateTime? LockoutStartTime { get; set; }

        [DisplayName("账户锁定持续时间（分钟）")]
        public int LockoutDuration { get; set; }

        [DisplayName("账户解锁时间"), MyDate]
        public DateTime? LockoutEndTime
        {
            get
            {
                if (LockoutStartTime.HasValue) return this.LockoutStartTime.Value.AddMinutes(this.LockoutDuration);
                else return null;
            }
        }

        [MyMaxLength(200), NonQuery]
        public string OperateToken { get; set; }
        #endregion

        [DisplayName("姓名"), MyMaxLength(20)]
        public string Name { get; set; }

        [DisplayName("手机"),MyMaxLength(20),MyPhone]
        public string MobileNumber { get; set; }

        [DisplayName("其他联系电话"), MyMaxLength(20), MyPhone]
        public string TelephoneNumber { get; set; }

        [DisplayName("证件号码"),MyMaxLength(50)]
        public string IDCardNumber { get; set; }

        [DisplayName("注册时间"),MyRequired]
        public DateTime? RegisterTime { get; set; }

        [DisplayName("账户状态")]
        public EnumUserStatus Status { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }

    }
}
