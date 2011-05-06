using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public class Organization : Subject
    {
        /// <summary>
        /// 上级组织
        /// </summary>
        public virtual Organization Parent { get; set; }

        /// <summary>
        /// 子组织,不含子组织的子组织
        /// </summary>
        public virtual ICollection<Organization> Children { get; set; }

        /// <summary>
        /// 组织负责人,可以是本组织以外的用户
        /// </summary>
        public virtual User Manager { get; set; }

        /// <summary>
        /// 组织内用户,不含子组织的用户
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
    }

    public partial class User
    {
        /// <summary>
        /// 用户所在组织,用户只能存在于最多一个组织中
        /// </summary>
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// 用户管理的组织,有可能不是本组织,适应兼管其他组织的情形
        /// </summary>
        public virtual ICollection<Organization> ManageOrgs { get; set; }
    }
}
