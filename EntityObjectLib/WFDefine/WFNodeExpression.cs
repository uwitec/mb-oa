using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// XORSplit节点表达式
    /// </summary>
    public class WFNodeExpression
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 条件表达式
        /// 运行后返回true或false
        /// 表达式规则,例如:{费用金额}>1000 and {费用类别}=="办公用品采购"
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 条件描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 表达式所在节点
        /// </summary>
        public virtual WFNodeXORSplit WFNode { get; set; }

        /// <summary>
        /// 表达式为真时,向此节点迁移
        /// </summary>
        public virtual WFNode Next { get; set; }

        /// <summary>
        /// 顺序号,根据此顺序进行表达式判断,遇到第一个为真的就跳转,即短路
        /// </summary>
        public int OrderNO { get; set; }

        public WFNodeExpression()
        {
            this.ID = Guid.NewGuid().ToString();
        }
    }

    public partial class WFNodeXORSplit
    {
        /// <summary>
        /// 本节点的表达式列表
        /// </summary>
        public virtual ICollection<WFNodeExpression> WFNodeExpressions { get; set; }
    }

    public partial class WFNode
    {
        /// <summary>
        /// 指向本节点的活动列表
        /// </summary>
        public virtual ICollection<WFNodeExpression> FromExpressions { get; set; }
    }
}
