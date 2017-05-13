using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Models
{
    public class Label
    {
        public Guid Id { get; set; }

        #region 导航属性===========================================================================
        public Guid PictureId { get; set; }
        public virtual Picture Picture { get; set; }
        #endregion=================================================================================
        [DisplayName("标签名")]
        public String Name { get; set; }

        [DisplayName("完成时间")]
        public DateTime FinishTime { get; set; }

        [DisplayName("有效性")]
        public bool? Effective { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }

    }
}