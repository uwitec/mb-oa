using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// 工作流节点活动,一个节点有多个活动
    /// 在本项目中, 活动对应节点间的路径
    /// </summary>
    public class Action
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 名称,用于界面控件显示
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名称,关联到代码
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 活动所在节点
        /// </summary>
        public virtual Node SelfNode { get; set; }

        /// <summary>
        /// 下一个节点
        /// </summary>
        public virtual Node NextNode { get; set; }
    }

    public partial class Node
    {
        /// <summary>
        /// 本节点的活动列表
        /// </summary>
        public virtual ICollection<Action> Actions { get; set; }

        /// <summary>
        /// 指向本节点的活动列表
        /// </summary>
        public virtual ICollection<Action> FromActions { get; set; }
    }
}
