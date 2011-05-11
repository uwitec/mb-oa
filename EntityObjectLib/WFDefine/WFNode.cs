using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib.WF
{
    /// <summary>
    /// 通用工作流节点,一个模板有多个节点,
    /// </summary>
    public partial class WFNode
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 节点扩展类型:
        /// </summary>
        public string ExtType { get; set; }

        /// <summary>
        /// 节点在图形上的位置X坐标
        /// </summary>
        public int PositionX { get; set; }

        /// <summary>
        /// 节点在图形上的位置Y坐标
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        /// 本节点所在的模板
        /// </summary>
        public virtual WFTemplate WFTemplate { get; set; }

        /// <summary>
        /// 是否会签
        /// 2011.5.8 chw
        /// 当前会签是必须全部参与人员签署完毕后,整个节点才处理完成,处理结果按多数人的活动选择为节点活动
        /// </summary>
        public bool IsCountersign { get; set; }

        public WFNode()
        {
            this.PositionX = this.PositionY = 0;
            this.ExtType = this.GetType().Name;
        }

        //public void Init()
        //{
        //    WFInstNode inode = new WFInstNode
        //    {
        //        ID = Guid.NewGuid().ToString(),
        //        WFNode = this,
        //       EntryTime = DateTime.Now,
        //        PreviouInstNode = null,
        //         State = "未处理",
        //          WFInst=null
        //    };
        //}

        //public void Activate()
        //{

        //}

        //public void Compelete()
        //{

        //}
    }

    public partial class WFTemplate
    {
        /// <summary>
        /// 模板的节点表
        /// </summary>
        public virtual ICollection<WFNode> Nodes { get; set; }
    }

}

