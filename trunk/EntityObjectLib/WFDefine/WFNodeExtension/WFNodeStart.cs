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
        public string ID { get; set; }
    }
}