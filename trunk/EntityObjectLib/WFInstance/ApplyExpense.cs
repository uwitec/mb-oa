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
        /// <summary>
        /// 费用金额
        /// </summary>
        [Display(Name="费用金额(元)")]
        public int Amount { get; set; }
        
        /// <summary>
        /// 用途说明
        /// </summary>
        [Display(Name = "用途说明")]
        public string Description { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public virtual User Applicant { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }

        public ApplyExpense()
        {
            // 测试业务属性用
            this.Description = "Description";
            
            this.ApplyTime = DateTime.Now;
        }
    }

    public partial class User
    {
        public virtual ICollection<ApplyExpense> CreateApplyExpenses { get; set; }
    }
}
