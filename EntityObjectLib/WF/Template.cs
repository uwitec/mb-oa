using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// 工作流模板
    /// </summary>
    public partial class Template
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 业务类
        /// </summary>
        [NotMapped]
        public IWFBuiz BuizType { get; set; }

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
        public virtual ICollection<EntityObjectLib.WF.Template> CreateWFTs { get; set; }
    }
}