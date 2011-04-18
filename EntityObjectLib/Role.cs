using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EntityObjectLib
{
    public class Role
    {
        [Key]
        public string ID { get; set; }

        public string roleCode { get; set; }

        public string roleName { get; set; }

        public string roleDescription { get; set; }

        public int? orderNO { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        /// <summary>
        /// The specified type member 'Users' is not supported in LINQ to Entities. 
        /// Only initializers, entity members, and entity navigation properties are supported.
        /// 在linq表达式中使用时用.OfType<User>()替代，在表达式外仍可使用
        /// 可参考：http://damieng.com/blog/2009/06/24/client-side-properties-and-any-remote-linq-provider
        /// </summary>
        [NotMapped]
        public ICollection<User> Users
        {
            get
            {
                return this.Subjects.OfType<User>().ToList();
            }
        }

        [NotMapped]
        public ICollection<Organization> Organizations
        {
            get
            {
                return this.Subjects.Where(s => s.Category.Equals("Organization")).Select(s => s as Organization).ToList();
            }
        }

        public virtual ICollection<RolePrivilege> RolePrivileges { get; set; }
    }
}
