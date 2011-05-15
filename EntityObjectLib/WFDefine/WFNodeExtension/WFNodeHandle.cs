using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// 顺序处理节点,可能是人工处理,也可能是自动处理 或条件(触发)处理
    /// </summary>
    public partial class WFNodeHandle : WFNode
    {
        /// <summary>
        /// 是否会签
        /// 2011.5.8 chw
        /// 当前会签是必须全部参与人员签署完毕后,整个节点才处理完成,处理结果按多数人的活动选择为节点活动
        /// </summary>
        public bool IsCountersign { get; set; }

        /// <summary>
        /// 视图代码
        /// </summary>
        public string ViewCode { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// 节点处理 人(主体)
        /// 这里有一个设计约束,如果有多个处理用户,则他们同等对待,共享活动
        /// 即,以下情形不予支持
        ///     如果该节点为的处理人u1,u2, u1可以执行a,b,c, u2只可以执行a,b
        /// </summary>
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}

namespace EntityObjectLib
{
    public partial class Subject
    {
        /// <summary>
        /// 主体参与的流程节点
        /// </summary>
        public virtual ICollection<EntityObjectLib.WF.WFNodeHandle> WFNodes { get; set; }
    }
}