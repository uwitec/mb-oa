using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 上传文件总表
    /// </summary>
    public class EventState
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 事件
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// 该状态发生的时间
        /// </summary>
        public DateTime StateTime { get; set; }

        /// <summary>
        /// 计划完成比例
        /// </summary>
        public double PlanRadio { get; set; }

        /// <summary>
        /// 实际完成比例
        /// </summary>
        public double AcutalRadio { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual User Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }

    public partial class User
    {
        /// <summary>
        /// 用户创建的事件
        /// </summary>
        public virtual ICollection<EventState> CreateEventStates { get; set; }
    }
}
