using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // 添加此引用后，才可以使用Required、Key等特性

namespace EntityObjectLib
{
    public class Movie
    {
        [Key]
        public string MPK { get; set; }

        [Required(ErrorMessage="")]
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }

        [Range(0.001,double.MaxValue)]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "")] // 指定必需
        public string CategoryCode { get; set; }

        public virtual Category MCategory { get; set; }
    }
}
