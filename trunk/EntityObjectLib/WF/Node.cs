using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// 通用工作流节点,一个模板有多个节点,
    /// </summary>
    public partial class Node
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 节点类型:开始(S),结束(F),条件分支(S),并发分支(P),合并(C),处理(H)
        /// </summary>
        public string Type { get; set; }

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
        public virtual Template Template { get; set; }
    }

    public partial class Template
    {
        /// <summary>
        /// 模板的节点表
        /// </summary>
        public virtual ICollection<Node> Nodes { get; set; }
    }
}