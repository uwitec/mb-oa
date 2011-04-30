using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 上传文件总表
    /// </summary>
    public class AddressBook
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// 地址,可以是家庭地址或办公地址,多个用逗号隔开
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 家庭电话号码,多个用逗号隔开
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// 办公电话号码,多个用逗号隔开
        /// </summary>
        public string OfficePhone { get; set; }

        /// <summary>
        /// 手机,多个用逗号隔开
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// MSN
        /// </summary>
        public string MSN { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual User Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 拥有者,拥有者可以管理此记录
        /// </summary>
        public virtual User Owner { get; set; }

        /// <summary>
        /// 最近一次更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 通讯录记录共享表
        /// </summary>
        public virtual ICollection<AddressBookShare> AddressBookShares { get; set; }
    }

    public partial class User
    {
        /// <summary>
        /// 用户创建的通讯录记录
        /// </summary>
        public virtual ICollection<AddressBook> CreateAddressBooks { get; set; }

        /// <summary>
        /// 用户管理的通讯录记录
        /// </summary>
        public virtual ICollection<AddressBook> OwnAddressBooks { get; set; }
    }
}
