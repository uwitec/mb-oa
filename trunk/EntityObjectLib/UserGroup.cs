using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 用户组
    /// 在用户组表示一个临时项目组时,需要用户组负责人的概念,简化处理的方法是按OrderNO排序,该用户组的第一人默认为用户组负责人
    /// </summary>
    public partial class UserGroup : Subject
    {
        //public string QQ { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    public partial class User
    {
        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
