using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// 工作流模板
    /// </summary>
    public partial class WFTemplate
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 业务类编码
        /// </summary>
        public string BuizCode { get; set; }

        /// <summary>
        /// 业务类名称
        /// </summary>
        public string BuizName { get; set; }

        public int OffsetX { get; set; }

        public int OffsetY { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual User Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}

namespace EntityObjectLib
{
    public partial class User
    {
        /// <summary>
        /// 用户创建的工作流模板
        /// </summary>
        public virtual ICollection<EntityObjectLib.WF.WFTemplate> CreateWFTemplates { get; set; }
    }
}