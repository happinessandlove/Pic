using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Picture
    {
        public Picture()
        {
            this.Labels = new List<Label>();
        }
        public Guid Id { get; set; }

#region 导航属性========================================================
        public virtual List<Label> Labels { get; set; }
#endregion==============================================================
        public string Name { get; set; }

        public string Url { get; set; }

        

       

    }
}