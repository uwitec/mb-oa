using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// 工作流节点活动,一个节点有多个活动
    /// 在本项目中, 活动对应节点间的路径
    /// </summary>
    public class WFNodeAction
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 名称,用于界面控件显示
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 活动所在节点
        /// </summary>
        public virtual WFNode WFNode { get; set; }

        /// <summary>
        /// 该活动允许根据业务表单的修改更新业务数据吗?
        /// 大多数情况下,应该允许更新业务数据
        /// 暂时不用,默认情况下都更新业务数据
        /// </summary>
        //public bool AllowUpdate { get; set; }

        /// <summary>
        /// 下一个节点
        /// 可以为空,表示节点内活动,不产生流转
        /// </summary>
        public virtual WFNode NextNode { get; set; }
    }

    public partial class WFNode
    {
        /// <summary>
        /// 本节点的活动列表
        /// </summary>
        public virtual ICollection<WFNodeAction> Actions { get; set; }

        /// <summary>
        /// 指向本节点的活动列表
        /// </summary>
        public virtual ICollection<WFNodeAction> FromActions { get; set; }
    }
}
