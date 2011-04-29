using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 收件箱
    /// </summary>
    public class InfoInbox
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public virtual Info Info { get; set; }

        public string ReceiveTypes { get; set; }

        public DateTime? ReadDate { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public virtual User Receiver { get; set; }
    }

    public partial class User
    {
        /// <summary>
        /// 用户接收的消息
        /// </summary>
        public virtual ICollection<InfoInbox> InfoInboxs { get; set; }
    }
}
