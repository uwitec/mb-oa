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
        /// 发文类型表
        /// </summary>
        public DbSet<FWType> FWTypes { get; set; }
        
        /// <summary>
        /// 发文模板表
        /// </summary>
        public DbSet<FWTemplate> FWTemplates { get; set; }
        
        /// <summary>
        /// 发文数据
        /// </summary>
        public DbSet<FWInfo> FWInfos { get; set; }
        
        /// <summary>
        /// 发文附件
        /// </summary>
        public DbSet<FWInfoFile> FWInfoFiles { get; set; }

        /// <summary>
        /// 流程模板模型
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void FWModelCreate(DbModelBuilder modelBuilder)
        {
            // 发文模板关联发方类型的关系
            modelBuilder.Entity<FWTemplate>()
                .HasRequired(p => p.FWType)
                .WithMany(u => u.FWTemplates)
                .Map(m => { m.MapKey("FWType"); });

            // 发文模板与文件的关系
            modelBuilder.Entity<FWTemplate>()
                .HasRequired(r => r.TemplateFile)
                .WithMany(m => m.FWTemplates)
                .Map(m => { m.MapKey("TemplateFile"); });

            // 发文与发文类型的关系
            modelBuilder.Entity<FWInfo>()
                .HasRequired(n => n.FWType)
                .WithMany(acl => acl.FWInfos)
                .Map(m => m.MapKey("FWType"));

            // 发文与发文模板的关系
            modelBuilder.Entity<FWInfo>()
                .HasRequired(n => n.FWTemplate)
                .WithMany(a => a.FWInfos)
                .Map(m => m.MapKey("FWTemplate"));

            // 发文与拟稿人的关系
            modelBuilder.Entity<FWInfo>()
                .HasRequired(a => a.Drafter) //如果为空,表示节点内活动
                .WithMany(n => n.FWInfos)
                .Map(m => m.MapKey("Drafter"));

            // 发文与发文附件的关系
            modelBuilder.Entity<FWInfo>()
                .HasMany(p => p.FWInfoFiles)
                .WithRequired(t => t.FWInfo)
                .Map(m => m.MapKey("FWInfo"));

            // 发文附件与文件的关系
            modelBuilder.Entity<FWInfoFile>()
                .HasRequired(p => p.File)
                .WithMany(t => t.FWInfoFiles)
                .Map(m => m.MapKey("File"));
            
        }
    }
}
