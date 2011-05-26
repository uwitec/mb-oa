using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 发文附件
    /// </summary>
    public class FWInfoFile
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 附件所在的发文
        /// </summary>
        public virtual FWInfo FWInfo { get; set; }

        /// <summary>
        /// 附件文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// 附件所含的文件
        /// </summary>
        public virtual File File { get; set; }
    }

    public partial class FWInfo
    {
        public virtual ICollection<FWInfoFile> FWInfoFiles { get; set; }
    }

    public partial class File
    {
        public virtual ICollection<FWInfoFile> FWInfoFiles { get; set; }
    }
}
