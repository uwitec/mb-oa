using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public partial class User : Subject
    {      
        public string Password { get; set; }

        public string Email { get; set; }

        public string QQ { get; set; }

        public string MSN { get; set; }

        public string OfficePhone { get; set; }

        public string HomePhone { get; set; }

        public string Mobile { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string InfoReceiveTypes { get; set; }

        public string LayoutData { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Format("{0}[{1}]", this.Name, this.ID);
            }
        }
    }
}
