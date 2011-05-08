using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// 并行分支
    /// 分解成多个可并行处理的节点实例
    /// </summary>
    public partial class WFNodeAndSplit : WFNode
    {
        public string ID { get; set; }
    }
}