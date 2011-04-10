using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    public class Category
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; }
    }
}
