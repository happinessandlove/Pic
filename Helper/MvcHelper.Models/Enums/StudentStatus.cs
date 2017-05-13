using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum StudentStatus
    {
        [Description("在籍描述")]
        在籍 = 1,

        [Description("已毕业")]
        毕业 = 2,

        [Description("参军")]
        参军 = 3
    }
}
