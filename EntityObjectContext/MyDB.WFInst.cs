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
        /// 流程实例表
        /// </summary>
        public DbSet<WFInst> WFInsts { get; set; }
        
        /// <summary>
        /// 流程实例节点表
        /// </summary>
        public DbSet<WFInstNode> WFInstNodes { get; set; }
        
        /// <summary>
        /// 流程实例节点操作表
        /// </summary>
        public DbSet<WFInstNodeHandler> WFInstNodeHandlers { get; set; }

        /// <summary>
        /// 流程实例模型
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void WFInstModelCreate(DbModelBuilder modelBuilder)
        {
            // 流程实例与流程模板的关系
            modelBuilder.Entity<WFInst>()
                .HasRequired(i => i.WFTemplate)
                .WithMany(t => t.WFInsts)
                .Map(m => { m.MapKey("WFTemplate"); });

            // 流程实例的创建者
            modelBuilder.Entity<WFInst>()
                .HasRequired(r => r.Creator)
                .WithMany(u => u.CreateWFInsts)
                .Map(m => { m.MapKey("Creator"); });

            // 流程实例节点与流程实例
            modelBuilder.Entity<WFInstNode>()
                .HasRequired(n => n.WFInst)
                .WithMany(i => i.WFInstNodes)
                .Map(m => m.MapKey("WFInst"));

            // 流程实例节点与流程模板节点
            modelBuilder.Entity<WFInstNode>()
                .HasRequired(n => n.WFNode)
                .WithMany(n => n.WFInstNodes)
                .Map(m => m.MapKey("WFNode"));

            // 流程实例节点处理人与流程实例节点
            modelBuilder.Entity<WFInstNodeHandler>()
                .HasRequired(h => h.WFInstNode)
                .WithMany(n => n.WFInstNodeHandlers)
                .Map(m => m.MapKey("WFInstNode"));

            // 流程实例节点处理人与用户
            modelBuilder.Entity<WFInstNodeHandler>()
                .HasRequired(h => h.Handler)
                .WithMany(u => u.WFInstNodeHandlers)
                .Map(m => m.MapKey("Handler"));
        }
    }
}
