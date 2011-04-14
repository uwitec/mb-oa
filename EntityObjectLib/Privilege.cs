using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public class Privilege
    {
        [Key]
        public string ID { get; set; }

        public string privilegeCode { get; set; }

        [Required(ErrorMessage = "privilegeName required")]
        public string privilegeName { get; set; }

        /// <summary>
        /// 是否需要认证
        /// </summary>
        public bool needAuth { get; set; }

        public bool isMenuEntry { get; set; }

        public DateTime createdTime { get; set; }

        public int securityGrade { get; set; }

        public int orderNO { get; set; }

        public virtual ICollection<RolePrivilege> PrivilegeRoles { get; set; }

        /// 下面的定义是不必要的，可以使用Map(m => { m.MapKey("resourceID"); })直接指定字段名
        //[Required(ErrorMessage = "resourceID required")] //必须指定
        //public string resourceID { get; set; }

        public virtual Resource resource { get; set; }
    }
}
