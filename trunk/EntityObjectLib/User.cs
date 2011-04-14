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

        public virtual Organization Organization { get; set; }

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
