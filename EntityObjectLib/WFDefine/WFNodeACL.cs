﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    // 业务数据控制类型
    public enum EnumACL
    {
        不可读 = 0, // 不可读
        可读,           // 可读,不可写
        可新建,         // 如果数据为空,可创建,如果不空,不可写
        可改写          // 可创建和改写
    }

    /// <summary>
    /// 节点的业务数据操作控制列表
    /// </summary>
    public class WFNodeACL
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 业务数据代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 业务数据名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 访问控制
        /// </summary>
        public string ACL { get; set; }

        /// <summary>
        /// 活动所在节点
        /// 设计约束:类似活动执行人的选择,在同一节点上,视图数据的控制对所有执行人是同等的
        /// </summary>
        public virtual WFNodeHandle WFNode { get; set; }

        public WFNodeACL()
        {
            this.ID = Guid.NewGuid().ToString();
        }
    }

    public partial class WFNodeHandle
    {
        /// <summary>
        /// 本节点的活动列表
        /// </summary>
        public virtual ICollection<WFNodeACL> NodeACLs { get; set; }
    }
}
