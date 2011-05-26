using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 发文类别
    /// </summary>
    public partial class FWType
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 发文代字,以逗号分隔
        /// </summary>
        public string FWPrefixes { get; set; }
    }
}
