using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 公告板
    /// </summary>
    public class InfoBoard
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 本板的管理员
        /// </summary>
        public virtual User Administrator { get; set; }

        /// <summary>
        /// 创建本板的用户名(直接取名称)
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 本板的消息列表
        /// </summary>
        public virtual ICollection<Info> Infos { get; set; }
    }
}
