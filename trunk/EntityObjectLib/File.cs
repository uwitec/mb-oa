using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 上传文件总表
    /// </summary>
    public partial class File
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Suffix { get; set; }

        public string UploadPath { get; set; }

        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 消息创建人
        /// </summary>
        public virtual User Creator { get; set; }

        /// <summary>
        /// 使用其的消息附件
        /// </summary>
        public virtual ICollection<InfoFile> InfoFiles { get; set; }
    }

    public partial class User
    {
        /// <summary>
        /// 用户上传的文件
        /// </summary>
        public virtual ICollection<File> Files { get; set; }
    }
}
