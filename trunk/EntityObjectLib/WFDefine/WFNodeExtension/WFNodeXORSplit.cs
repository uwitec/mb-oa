using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// Exclusive Choice (异或分支)
    /// 只能选择一个后继节点
    /// </summary>
    public partial class WFNodeXORSplit : WFNode
    {
        /// <summary>
        /// 条件表达式
        /// 运行后返回true或false,对应活动中的"是"和"否"
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 开始节点的后继节点,有且只有一个
        /// 这个关系是单向的
        /// </summary>
        public virtual WFNode Next { get; set; }
    }
}