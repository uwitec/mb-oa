using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// 通用工作流节点,一个模板有多个节点,
    /// </summary>
    public partial class WFNode
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 视图代码
        /// </summary>
        public string ViewCode { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// 节点扩展类型:
        /// </summary>
        public string ExtType { get; set; }

        /// <summary>
        /// 节点在图形上的位置X坐标
        /// </summary>
        public int PositionX { get; set; }

        /// <summary>
        /// 节点在图形上的位置Y坐标
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        /// 本节点所在的模板
        /// </summary>
        public virtual WFTemplate WFTemplate { get; set; }

        /// <summary>
        /// 节点处理 人(主体)
        /// </summary>
        public virtual ICollection<Subject> Subjects { get; set; }

        public WFNode()
        {
            this.ExtType = this.GetType().Name;
        }
    }

    public partial class WFTemplate
    {
        /// <summary>
        /// 模板的节点表
        /// </summary>
        public virtual ICollection<WFNode> Nodes { get; set; }
    }

}

namespace EntityObjectLib
{
    public partial class Subject
    {
        /// <summary>
        /// 主体参与的流程节点
        /// </summary>
        public virtual ICollection<EntityObjectLib.WF.WFNode> WFNodes { get; set; }
    }
}