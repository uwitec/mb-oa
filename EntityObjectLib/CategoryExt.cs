using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // 添加此引用后，才可以使用Required、Key等特性

namespace EntityObjectLib
{
    public class CategoryExt : Category
    {
        public string ExtName { get; set; }
    }
}
