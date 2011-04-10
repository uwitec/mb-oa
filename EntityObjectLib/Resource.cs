using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public class Resource
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        [StringLength(200,ErrorMessage="dddd",MinimumLength=1)]
        public string resourceCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        [Required(ErrorMessage = "Resource required")]
        public string resourceName { get; set; }

        public int? orderNO { get; set; }

        public string moduleID { get; set; }

        [Association("Resource_module", "moduleID", "ID", IsForeignKey = true)] //moduleID必须存在
        public virtual Module module { get; set; }

        public virtual ICollection<Privilege> privileges { get; set; }
    }
}
