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
        /// 缺省迁移节点,如果所有条件都不满足,则向该节点迁移
        /// </summary>
        public virtual WFNode Next { get; set; }
    }
}