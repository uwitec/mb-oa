using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public class Module
    {
        [Key]
        public string ID { get; set; }
        
        /// <summary>
        /// 模块编码
        /// </summary>
        public string moduleCode { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string moduleName { get; set; }

        /// <summary>
        /// 模块描述
        /// </summary>
        public string moduleDescription { get; set; }

        public virtual ICollection<Resource> resources { get; set; }
    }
}
