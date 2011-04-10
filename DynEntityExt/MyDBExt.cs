using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using EntityObjectLib; 

namespace EntityObjectContext
{
    public class MyDBExt : MyDB
    {
        public DbSet<LinkMethod> LinkMethods { get; set; }

        //public DbSet<UserForLinkMethod> UserForLinkMethods { get; set; } 

        public MyDBExt() { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<LinkMethod>()
            //        .HasRequired(p => p.user) //必须在resourceID设置特性Required
            //        .WithMany(p=>p.getLinkMethodsByUser())
            //        .Map(m => { m.MapKey("UserID"); });// 在Privilege中不定义resourceID，用该语句建立关联，建议这样做
        }   
    }
}
