using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 收件箱
    /// </summary>
    public class MyInfo
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public virtual Info Info { get; set; }

        public string ReceiveTypes { get; set; }

        public virtual DateTime ReadDate { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public virtual User Receiver { get; set; }
    }
}
