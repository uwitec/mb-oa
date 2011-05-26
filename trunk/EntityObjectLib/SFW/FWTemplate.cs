using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 发文模板
    /// </summary>
    public partial class FWTemplate
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 发文类别
        /// </summary>
        public virtual FWType FWType { get; set; }

        /// <summary>
        /// 模板文件
        /// </summary>
        public virtual File TemplateFile { get; set; }
    }

    public partial class FWType
    {
        public virtual ICollection<FWTemplate> FWTemplates { get; set; }
    }

    public partial class File
    {
        public virtual ICollection<FWTemplate> FWTemplates { get; set; }
    }
}
