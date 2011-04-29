using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 消息
    /// </summary>
    public class Info
    {
        [Key]
        public string ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime SendDate { get; set; }

        public string SendTypes { get; set; }

        /// <summary>
        /// 消息创建人
        /// </summary>
        public virtual User Creator { get; set; }

        /// <summary>
        /// 父消息
        /// </summary>
        public virtual Info Parent { get; set; }

        /// <summary>
        /// 子消息列表
        /// </summary>
        public virtual ICollection<Info> Children { get; set; }

        /// <summary>
        /// 所属的公告板
        /// </summary>
        public virtual InfoBoard Board { get; set; }

        /// <summary>
        /// 消息附件
        /// </summary>
        public virtual ICollection<InfoFile> InfoFiles { get; set; }

        /// <summary>
        /// 收件
        /// </summary>
        public virtual ICollection<InfoInbox> Receivers { get; set; }

        /// <summary>
        /// 参与的订阅
        /// </summary>
        public virtual ICollection<InfoSubscription> Subscriptions { get; set; }
        
    }

    public partial class User
    {
        /// <summary>
        /// 用户创建的消息
        /// </summary>
        public virtual ICollection<Info> Infos { get; set; }
    }
}
