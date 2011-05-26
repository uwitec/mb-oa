using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 发文信息
    /// </summary>
    public partial class FWInfo : WFInst
    {
        //[Key]
        //public string ID { get; set; }

        /// <summary>
        /// 发文类别
        /// </summary>
        public virtual FWType FWType { get; set; }

        /// <summary>
        /// 发文模板
        /// </summary>
        public virtual FWTemplate FWTemplate { get; set; }

        /// <summary>
        /// 发文代字
        /// </summary>
        public string FWPrefix { get; set; }

        /// <summary>
        /// 发文文号的年份
        /// </summary>
        public int FWYear { get; set; }

        /// <summary>
        /// 发文文号的序号
        /// </summary>
        public int FWNO { get; set; }

        /// <summary>
        /// 紧急程度
        /// </summary>
        public string Urgency { get; set; }

        /// <summary>
        /// 密级
        /// </summary>
        public string Secrecy { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 主题词,关键词
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 发文摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 主送机构
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        /// 抄送,分享者
        /// </summary>
        public string Participator { get; set; }

        /// <summary>
        /// 拟稿人
        /// </summary>
        public virtual User Drafter { get; set; }

        /// <summary>
        /// 拟稿时间
        /// </summary>
        public DateTime DraftTime { get; set; }

        /// <summary>
        /// 印发份数
        /// </summary>
        public int CopyAmount { get; set; }
    }

    public partial class FWType
    {
        public virtual ICollection<FWInfo> FWInfos { get; set; }
    }

    public partial class FWTemplate
    {
        public virtual ICollection<FWInfo> FWInfos { get; set; }
    }

    public partial class User
    {
        public virtual ICollection<FWInfo> FWInfos { get; set; }
    }
}
