using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 基于通用数据模型的业务实例
    /// </summary>
    public class GenericBuiz : WFInst
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Data { get; set; }

        public virtual GenericModel Model { get; set; }

        [NotMapped]
        public string ModelName
        {
            get
            {
                return this.Model.Name;
            }
        }

        public GenericBuiz()
        {
            this.ID = Guid.NewGuid().ToString();
        }
    }

    public partial class GenericModel
    {
        public virtual ICollection<GenericBuiz> Buizs { get; set; }
    }
}
