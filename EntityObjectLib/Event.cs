using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 上传文件总表
    /// </summary>
    public class Event
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 事件内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间(完成时间)
        /// </summary>
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// 父事件
        /// </summary>
        public virtual Event Parent { get; set; }

        /// <summary>
        /// 子事件
        /// </summary>
        public virtual ICollection<Event> Children { get; set; }

        /// <summary>
        /// 事件创建人
        /// </summary>
        public User Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public User Master { get; set; }

        /// <summary>
        /// 督办人
        /// </summary>
        public User Proctor { get; set; }

        /// <summary>
        /// 事件共享
        /// </summary>
        public ICollection<EventShare> EventShares { get; set; }

        /// <summary>
        /// 事件状态集
        /// </summary>
        public ICollection<EventState> EventStates { get; set; }

        /// <summary>
        /// 事件提醒
        /// </summary>
        public ICollection<EventRemind> EventReminds { get; set; }
    }

    public partial class User
    {
        /// <summary>
        /// 用户创建的事件
        /// </summary>
        public virtual ICollection<Event> CreateEvents { get; set; }

        /// <summary>
        /// 用户负责的事件
        /// </summary>
        public virtual ICollection<Event> MasterEvents { get; set; }

        /// <summary>
        /// 用户督办的事件
        /// </summary>
        public virtual ICollection<Event> ProctorEvents { get; set; }
    }
}
