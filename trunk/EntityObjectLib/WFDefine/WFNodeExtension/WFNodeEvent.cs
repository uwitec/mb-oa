using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityObjectLib.WF.NodeExtension
{
    /// <summary>
    /// 事件类节点,由事件触发
    /// 如:时间触发,到达某节点24小时后移入下一节点
    /// </summary>
    public partial class WFNodeEvent : WFNode
    {
        public string ID { get; set; }
    }
}
