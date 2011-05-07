using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;// 添加此引用后，才可以使用Required、Key等特性
using EntityObjectLib;

namespace EntityObjectContext
{
    public partial class MyDB : DbContext // MyDB是连接串的名称
    {
        /// <summary>
        /// 费用申请
        /// </summary>
        public DbSet<ApplyExpense> ApplyExpenses { get; set; }

        /// <summary>
        /// 流程实例模型
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void ApplyExpenseModelCreate(DbModelBuilder modelBuilder)
        {
            // 流程实例与流程模板的关系
            modelBuilder.Entity<ApplyExpense>()
                .HasRequired(i => i.Creator)
                .WithMany(t => t.CreateApplyExpenses)
                .Map(m => { m.MapKey("Creator"); });

            modelBuilder.Entity<ApplyExpense>().ToTable("ApplyExpenses");
        }
    }
}
