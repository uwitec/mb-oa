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
        public string ID { get; set; }

        /// <summary>
        /// 条件表达式
        /// 运行后返回true或false,对应活动中的"是"和"否"
        /// </summary>
        public string Expression { get; set; }
    }
}