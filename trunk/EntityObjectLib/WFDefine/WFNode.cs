﻿using System;
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
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 节点类型:
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
        public virtual WFTemplate WFTemplate { get; set; }

        public WFNode()
        {
            this.Type = this.GetType().Name;
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