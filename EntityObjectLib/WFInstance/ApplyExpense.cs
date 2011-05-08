using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityObjectLib
{
    /// <summary>
    /// 申请用款
    /// </summary>
    public class ApplyExpense : WFInst
    {
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public int Amount { get; set; }
        
        /// <summary>
        /// 用途说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public virtual User Applicant { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
    }

    public partial class User
    {
        public virtual ICollection<ApplyExpense> CreateApplyExpenses { get; set; }
    }
}
