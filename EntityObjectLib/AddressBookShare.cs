using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 上传文件总表
    /// </summary>
    public class AddressBookShare
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 通讯录记录
        /// </summary>
        public AddressBook AddressBook { get; set; }

        /// <summary>
        /// 共享主体
        /// </summary>
        public Subject Subject { get; set; }
    }

    public partial class Subject
    {
        public virtual ICollection<AddressBookShare> AddressBookShares { get; set; }
    }
}
