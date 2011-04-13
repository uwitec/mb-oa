using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public class Organization
    {
        [Key]
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int OrderNO { get; set; }

        public virtual Organization ParentOrganization { get; set; }
        public virtual ICollection<Organization> ChildOrganizations { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
