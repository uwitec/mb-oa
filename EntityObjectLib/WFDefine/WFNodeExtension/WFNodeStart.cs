using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// 开始节点,一个模板中只有一个
    /// </summary>
    public partial class WFNodeStart : WFNode
    {
        /// <summary>
        /// 开始节点的后继节点,有且只有一个
        /// 这个关系是单向的
        /// </summary>
        public virtual WFNode Next { get; set; }
    }
}