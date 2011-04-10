using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public class User
    {
        [Key]
        public string ID { get; set; }

        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string Password { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Format("{0}[{1}]", this.Name, this.ID);
            }
        }
    }
}
