using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;// 添加此引用后，才可以使用Required、Key等特性
using EntityObjectLib.WF;

namespace EntityObjectContext
{
    public partial class MyDB : DbContext // MyDB是连接串的名称
    {
        /// <summary>
        /// 流程模板表
        /// </summary>
        public DbSet<WFTemplate> WFTemplates { get; set; }
        
        /// <summary>
        /// 流程模板节点表
        /// </summary>
        public DbSet<WFNode> WFNodes { get; set; }
        
        /// <summary>
        /// 流程模板节点业务数据访问表
        /// </summary>
        public DbSet<WFNodeACL> WFNodeACLs { get; set; }
        
        /// <summary>
        /// 流程模板节点动作表
        /// </summary>
        public DbSet<WFNodeAction> WFNodeActions { get; set; }

        /// <summary>
        /// 流程模板模型
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void WFDefineModelCreate(DbModelBuilder modelBuilder)
        {
            // 工作流模板的创建人
            modelBuilder.Entity<WFTemplate>()
                .HasRequired(p => p.Creator)
                .WithMany(u => u.CreateWFTemplates)
                .Map(m => { m.MapKey("Creator"); });

            // 模板与节点关系
            modelBuilder.Entity<WFNode>()
                .HasRequired(r => r.WFTemplate)
                .WithMany(m => m.Nodes)
                .Map(m => { m.MapKey("WFTemplate"); });

            // 节点与其访问列表
            modelBuilder.Entity<WFNode>()
                .HasMany(n => n.NodeACLs)
                .WithRequired(acl => acl.WFNode)
                .Map(m => m.MapKey("WFNode"));

            // 节点与其活动列表
            modelBuilder.Entity<WFNode>()
                .HasMany(n => n.Actions)
                .WithRequired(a => a.WFNode)
                .Map(m => m.MapKey("WFNode"));

            // 活动及其后继节点
            modelBuilder.Entity<WFNodeAction>()
                .HasRequired(a => a.NextNode)
                .WithMany(n => n.Actions)
                .Map(m => m.MapKey("NextNode"));
        }
    }
}
