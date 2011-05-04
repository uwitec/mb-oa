using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 上传文件总表
    /// </summary>
    public class EventRemind
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 事件
        /// </summary>
        public virtual Event Event { get; set; }

        /// <summary>
        /// 提醒时间
        /// </summary>
        public DateTime RemindTime { get; set; }

        /// <summary>
        /// 提醒内容(模板)
        /// </summary>
        public string RemindContent { get; set; }

        /// <summary>
        /// 提醒目标类型:责任人,督办人,共享人
        /// </summary>
        public string ReceiverType { get; set; }

        /// <summary>
        /// 提醒发送时间
        /// 如果非空,表示提醒已经发出
        /// </summary>
        public DateTime? SendTime { get; set; }

    }
}
