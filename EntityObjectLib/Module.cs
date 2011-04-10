using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public class Module
    {
        [Key]
        public string ID { get; set; }

        public string moduleCode { get; set; }
        public string moduleName { get; set; }

        public virtual ICollection<Resource> resources { get; set; }
    }
}
