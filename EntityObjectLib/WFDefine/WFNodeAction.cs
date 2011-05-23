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
        /// 代码,在节点类型是XORSplit时,是一个表达式,可返回字符串与Name比较,符合则自动执行活动
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 活动的顺序
        /// 在节点类型是XORSplit时,按此顺序检测,遇到第一个符合的则执行活动
        /// </summary>
        //public int OrderNO { get; set; }

        /// <summary>
        /// 活动所在节点
        /// </summary>
        public virtual WFNodeHandle WFNode { get; set; }

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

        /// <summary>
        /// 中间点X坐标
        /// </summary>
        public int? PositionX { get; set; }

        /// <summary>
        /// 中点间Y坐标
        /// </summary>
        public int? PositionY { get; set; }

        public WFNodeAction()
        {
            this.ID = Guid.NewGuid().ToString();
        }
    }

    public partial class WFNodeHandle
    {
        /// <summary>
        /// 本节点的活动列表
        /// </summary>
        public virtual ICollection<WFNodeAction> Actions { get; set; }
    }

    public partial class WFNode
    {
        /// <summary>
        /// 指向本节点的活动列表
        /// </summary>
        public virtual ICollection<WFNodeAction> FromActions { get; set; }
    }
}
