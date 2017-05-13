using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 角色权限表
    /// </summary>
    public class Role
    {
        public Role()
        {
            this.Users = new List<User>();
        }

        public Guid Id { get; set; }

        #region 导航属性=====================================================================================
        public virtual List<User> Users { get; set; }
        #endregion===========================================================================================

        [DisplayName("角色名称"), MyRequired, MaxLength(20)]
        public string Name { get; set; }

        [DisplayName("权限"), MyRequired,NonQuery]
        public string MenuId { get; set; }

        [DisplayName("备注")]
        [MyMaxLength(100), DataType(DataType.MultilineText)]
        public string Remarks { get; set; }


    }
}
