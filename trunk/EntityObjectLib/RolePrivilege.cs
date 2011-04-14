using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EntityObjectLib
{
    public class RolePrivilege
    {
        [Key]
        public string ID { get; set; }

        public string Parameters { get; set; }

        public virtual Role Role { get; set; }

        public virtual Privilege Privilege { get; set; }
    }
}
