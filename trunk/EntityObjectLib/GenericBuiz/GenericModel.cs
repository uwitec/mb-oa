using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 通用数据模型
    /// </summary>
    public partial class GenericModel
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public GenericModel()
        {
            this.ID = Guid.NewGuid().ToString();
        }
    }
}
