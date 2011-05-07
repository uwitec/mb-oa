using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 节点处理项(用户)
    /// </summary>
    public class WFInstNodeHandler
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 流程实例节点
        /// </summary>
        public virtual WFInstNode WFInstNode { get; set; }

        /// <summary>
        /// 流程实例节点处理人
        /// </summary>
        public virtual User Handler { get; set; }

        /// <summary>
        /// 处理意见,对应动作
        /// </summary>
        public string Opinion { get; set; }

        /// <summary>
        /// 处理意见说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 处理状态:处理中,已处理
        /// 与节点状态不同,处理状态决定节点状态
        /// 判断一个节点是否要处理,由节点状态决定,不是由处理状态决定
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime HandleTime { get; set; }
    }

    public partial class WFInstNode
    {
        /// <summary>
        /// 流程实例节点的处理人集合
        /// </summary>
        public virtual ICollection<WFInstNodeHandler> WFInstNodeHandlers { get; set; }
    }

    public partial class User
    {
        /// <summary>
        /// 用户的处理事项,包括待处理和已处理的
        /// </summary>
        public virtual ICollection<WFInstNodeHandler> WFInstNodeHandlers { get; set; }
    }
}
