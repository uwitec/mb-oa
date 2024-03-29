﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 流程实例节点
    /// </summary>
    public partial class WFInstNode
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 流程实例
        /// </summary>
        public virtual WFInst WFInst { get; set; }

        /// <summary>
        /// 实例节点对应的模板节点
        /// </summary>
        public virtual WF.WFNode WFNode { get; set; }

        /// <summary>
        /// 当前节点的前一个节点
        /// </summary>
        //public WFInstNode PreviouInstNode { get; set; }

        /// <summary>
        /// 进入当前节点的时间
        /// </summary>
        public DateTime EntryTime { get; set; }
        
        /// <summary>
        /// 离开当前节点的时间
        /// </summary>
        public DateTime LeaveTime { get; set; }

        /// <summary>
        /// 处理情况摘要
        /// 开始节点: 无
        /// 处理结点: 节点名称:,处理人:,处理意见:
        /// 分支结点: 满足条件:
        /// 结束节点: 结束
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 节点状态:处理中,已处理
        /// </summary>
        public string State { get; set; }

        public WFInstNode()
        {
            this.ID = Guid.NewGuid().ToString();
            EntryTime = DateTime.Now;
            State = "未处理";
        }
    }

    public partial class WFInst
    {
        /// <summary>
        /// 流程实例的节点集
        /// </summary>
        public virtual ICollection<WFInstNode> WFInstNodes { get; set; }
    }
}

namespace EntityObjectLib.WF
{
    public partial class WFNode
    {
        /// <summary>
        /// 模板节点对应的实例节点
        /// </summary>
        public virtual ICollection<WFInstNode> WFInstNodes { get; set; }
    }
}
