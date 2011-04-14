using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public class Organization : Subject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int OrderNO { get; set; }

        public virtual Organization ParentOrganization { get; set; }
        public virtual ICollection<Organization> ChildOrganizations { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Organization()
        {
            //return; //要在此处实现对this.Category赋值
            this.Category = this.GetType().Name;
        }
    }
}
