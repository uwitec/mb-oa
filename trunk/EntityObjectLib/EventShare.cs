using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 上传文件总表
    /// </summary>
    public class EventShare
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 事件
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// 共享目标,可以是用户或组织
        /// </summary>
        public virtual Subject Subject { get; set; }

        /// <summary>
        /// 是否需要参与提醒
        /// </summary>
        public bool NeedRemind { get; set; }

    }

    public partial class Subject
    {
        /// <summary>
        /// 用户创建的事件
        /// </summary>
        public virtual ICollection<EventShare> EventShares { get; set; }
    }
}
