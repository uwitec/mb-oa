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
        /// 通用数据模型表
        /// </summary>
        public DbSet<GenericModel> GenericModels { get; set; }
        
        /// <summary>
        /// 视图表
        /// </summary>
        public DbSet<GenericView> GenericViews { get; set; }
        
        /// <summary>
        /// 通用业务实例表
        /// </summary>
        public DbSet<GenericBuiz> GenericBuizs { get; set; }

        /// <summary>
        /// 流程实例模型
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void GenericBuizModelCreate(DbModelBuilder modelBuilder)
        {
            // 模型与视图的关系
            modelBuilder.Entity<GenericView>()
                .HasRequired(i => i.Model)
                .WithMany(t => t.Views)
                .Map(m => { m.MapKey("Model"); });

            // 模型与实例的关系
            modelBuilder.Entity<GenericBuiz>()
                .HasRequired(r => r.Model)
                .WithMany(u => u.Buizs)
                .Map(m => { m.MapKey("Model"); });

            modelBuilder.Entity<GenericBuiz>().ToTable("GenericBuizs");
        }
    }
}
