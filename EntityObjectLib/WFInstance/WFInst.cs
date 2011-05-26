using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EntityObjectLib
{
    /// <summary>
    /// 工作流基类,所有支持工作流的实体类必须从此类继承
    /// </summary>
    public partial class WFInst
    {
        [Key]
        public string ID { get; set; }

        // 还需要一个实例的摘要或名称,暂未设定属性

        /// <summary>
        /// 流程实例当前状态,取活动状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 当前实例使用的流程模板
        /// </summary>
        public virtual WF.WFTemplate WFTemplate { get; set; }

        /// <summary>
        /// 流程实例创建人
        /// </summary>
        public virtual User Creator { get; set; }

        /// <summary>
        /// 流程实例创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public WFInst()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
            this.State = "处理中";
        }

        [NotMapped]
        public WFInstNode CurrentNode
        {
            get
            {
                return this.WFInstNodes.First(n => n.State.Equals("处理中"));
            }
        }
    }

    public partial class User
    {
        public virtual ICollection<WFInst> CreateWFInsts { get; set; }
    }
}

namespace EntityObjectLib.WF
{
    public partial class WFTemplate
    {
        public virtual ICollection<WFInst> WFInsts{get;set;}
    }
}
