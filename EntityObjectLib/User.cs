using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public class User : Subject
    {
        //[Key]
        //public string ID { get; set; }

        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string Password { get; set; }

        public string Email { get; set; }

        public string QQ { get; set; }

        public string MSN { get; set; }

        public string OfficePhone { get; set; }

        public string HomePhone { get; set; }

        public string Mobile { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string Description { get; set; }

        public string InfoReceiveTypes { get; set; }

        public virtual Organization Organization { get; set; }

        /// <summary>
        /// 用户创建的消息
        /// </summary>
        public virtual ICollection<Info> Infos { get; set; }

        /// <summary>
        /// 用户接收的消息
        /// </summary>
        public virtual ICollection<MyInfo> ReceiveInfos { get; set; }

        /// <summary>
        /// 用户管理的公告板
        /// </summary>
        public virtual ICollection<InfoBoard> InfoBoards { get; set; }

        /// <summary>
        /// 用户的订阅
        /// </summary>
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// 用户上传的文件
        /// </summary>
        public virtual ICollection<File> Files { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Format("{0}[{1}]", this.Name, this.ID);
            }
        }

        public User()
        {
            //return; //要在此处实现对this.Category赋值
            this.Category = this.GetType().Name;
        }
    }
}
