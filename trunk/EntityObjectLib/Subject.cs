using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 权限主体，用户、用户组、组织均由此继承
    /// Subject在这里是“主体”
    /// </summary>
    public partial class Subject
    {
        [Key]
        public string ID { get; set; }

        public string Category { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
