using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 消息订阅
    /// </summary>
    public class InfoSubscription
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 消息(主题)
        /// </summary>
        public virtual Info Title { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 该订阅的拥有者
        /// </summary>
        public virtual User Owner { get; set; }

        /// <summary>
        /// 是否可用,对应订阅和取消订阅
        /// </summary>
        public bool Enable { get; set; }
    }

    public partial class User
    {
        /// <summary>
        /// 用户的订阅
        /// </summary>
        public virtual ICollection<InfoSubscription> InfoSubscriptions { get; set; }
    }
}
