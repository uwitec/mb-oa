using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public class Organization
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual Organization ParentOrganization { get; set; }
        public virtual ICollection<Organization> ChildOrganizations { get; set; }
    }
}
