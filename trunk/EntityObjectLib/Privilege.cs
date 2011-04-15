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

        /// <summary>
        /// 是否是菜单入口
        /// </summary>
        public bool isMenuEntry { get; set; }

        public DateTime createdTime { get; set; }

        public int securityGrade { get; set; }

        /// <summary>
        /// 操作描述,包括请求的方式和数据格式
        /// </summary>
        public string privilegeDescription { get; set; }

        public int orderNO { get; set; }

        public virtual ICollection<RolePrivilege> PrivilegeRoles { get; set; }

        /// 下面的定义是不必要的，可以使用Map(m => { m.MapKey("resourceID"); })直接指定字段名
        //[Required(ErrorMessage = "resourceID required")] //必须指定
        //public string resourceID { get; set; }

        public virtual Resource resource { get; set; }
    }
}
