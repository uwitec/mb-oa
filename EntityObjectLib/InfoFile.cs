using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 消息附件
    /// </summary>
    public class InfoFile
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 附件所在消息
        /// </summary>
        public virtual Info Info { get; set; }

        public string FileName { get; set; }

        public DateTime UploadDate { get; set; }

        /// <summary>
        /// 附件所含的文件
        /// </summary>
        public virtual File File { get; set; }
    }
}
