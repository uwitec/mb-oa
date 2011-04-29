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
        /// 事件表
        /// </summary>
        public DbSet<Event> Events { get; set; }

        /// <summary>
        /// 事件共享表
        /// </summary>
        public DbSet<EventShare> EventShares { get; set; }

        /// <summary>
        /// 事件状态表
        /// </summary>
        public DbSet<EventState> EventStates { get; set; }

        /// <summary>
        /// 事件提醒表
        /// </summary>
        public DbSet<EventRemind> EventReminds { get; set; }

        private void EventModelCreate(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasRequired(e => e.Creator)
                .WithMany(u => u.CreateEvents)
                .Map(m => { m.MapKey("Creator"); });

            modelBuilder.Entity<Event>()
                .HasRequired(e => e.Master)
                .WithMany(u => u.MasterEvents)
                .Map(m => { m.MapKey("Master"); });

            modelBuilder.Entity<Event>()
                .HasRequired(e => e.Proctor)
                .WithMany(u => u.ProctorEvents)
                .Map(m => { m.MapKey("Proctor"); });

            modelBuilder.Entity<Event>()
                .HasOptional(e => e.Parent)
                .WithMany(e => e.Children)
                .Map(e => e.MapKey("ParentID"));

            modelBuilder.Entity<Event>()
                .HasMany(e=>e.EventShares)
                .WithOptional(e=>e.Event)
                .Map(m => { m.MapKey("EventID"); });

            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventStates)
                .WithOptional(e => e.Event)
                .Map(m => { m.MapKey("EventID"); });

            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventReminds)
                .WithOptional(e => e.Event)
                .Map(m => { m.MapKey("EventID"); });

            modelBuilder.Entity<EventShare>()
                .HasRequired(e => e.Subject)
                .WithMany(s => s.EventShares)
                .Map(m => { m.MapKey("SubjectID"); });

            modelBuilder.Entity<EventState>()
                .HasRequired(e => e.Creator)
                .WithMany(u => u.CreateEventStates)
                .Map(m => { m.MapKey("Creator"); });
        }
    }
}
