using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// 同步合并
    /// 多路必须全部完成,才能进入下一节点
    /// </summary>
    public partial class WFNodeAndJoin : WFNode
    {
        public string ID { get; set; }
    }
}