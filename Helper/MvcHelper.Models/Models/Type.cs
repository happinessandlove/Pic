using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
{
    public class Type
    {
        public Type()
        {
            this.Pictures = new List<Picture>();
            this.Volunteers = new List<Volunteer>();
        }
        [Key]
        public Guid Id { get; set; }
#region 导航属性==============================================================
        
        public virtual List<Picture> Pictures { get; set; }
        public Guid VolunteerId { get; set; }
        public virtual List<Volunteer> Volunteers { get; set; }

#endregion====================================================================
        [DisplayName("种类名称")]
        public String Name { get; set; }
    }
}