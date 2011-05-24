using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 基于通用数据模型的视图
    /// </summary>
    public class GenericView
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public virtual GenericModel Model { get; set; }

        public GenericView()
        {
            this.ID = Guid.NewGuid().ToString();
        }
    }

    public partial class GenericModel
    {
        public virtual ICollection<GenericView> Views { get; set; }
    }
}
