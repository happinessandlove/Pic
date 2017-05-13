using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
{
    public class VolunteerToken
    {
        [Key]
        public Guid VolunteerId { get; set; }

        [Index]
        [DisplayName("Session Key"),MyRequired,MyMaxLength(900)]
        public string Token { get; set; }

        [DisplayName("创建时间"),MyRequired]
        public DateTime CreateTime { get; set; }

        [DisplayName("激活时间"),MyRequired]
        public DateTime ActiveTime { get; set; }

        [DisplayName("有效期(天)"),MyRequired]
        public int Validity { get; set; }

        [DisplayName("到期时间"),MyRequired]
        public DateTime ExpiredTime { get { return this.ActiveTime.AddDays(this.Validity); } }
        
    }
}