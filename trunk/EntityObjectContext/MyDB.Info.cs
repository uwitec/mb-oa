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
        // 消息管理
        public DbSet<Info> Infos { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<InfoInbox> InfoInboxs { get; set; }

        public DbSet<InfoFile> InfoFiles { get; set; }

        public DbSet<InfoBoard> InfoBoards { get; set; }

        public DbSet<InfoSubscription> InfoSubscriptions { get; set; }

        private void InfoModelCreate(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Info>()
                .HasRequired(i => i.Creator) 
                .WithMany(u => u.Infos)
                .Map(m => { m.MapKey("Creator"); });

            modelBuilder.Entity<Info>()
                .HasOptional(i => i.Board)
                .WithMany(b => b.Infos)
                .Map(m => { m.MapKey("BoardID"); });

            modelBuilder.Entity<Info>()
                .HasOptional(i => i.Parent)
                .WithMany(i => i.Children)
                .Map(m => { m.MapKey("ParentID"); });
            modelBuilder.Entity<Info>().ToTable("Infos");

            modelBuilder.Entity<InfoFile>()
                .HasRequired(f => f.Info)
                .WithMany(i => i.InfoFiles)
                .Map(m => { m.MapKey("InfoID"); });

            modelBuilder.Entity<InfoFile>()
                .HasRequired(f => f.File)
                .WithMany(f => f.InfoFiles)
                .Map(m => { m.MapKey("FileID"); });

            modelBuilder.Entity<File>()
                .HasRequired(f => f.Creator)
                .WithMany(u => u.Files)
                .Map(m => { m.MapKey("Creator"); });

            modelBuilder.Entity<InfoInbox>().ToTable("InfoInboxs");
            modelBuilder.Entity<InfoInbox>()
                .HasRequired(f => f.Info)
                .WithMany(i => i.Receivers)
                .Map(m => { m.MapKey("InfoID"); });

            modelBuilder.Entity<InfoInbox>()
                .HasRequired(f => f.Receiver)
                .WithMany(u => u.InfoInboxs)
                .Map(m => { m.MapKey("Receiver"); });

            modelBuilder.Entity<InfoBoard>()
                .HasRequired(b => b.Administrator)
                .WithMany(u => u.InfoBoards)
                .Map(m => { m.MapKey("Administrator"); });

            modelBuilder.Entity<InfoSubscription>()
                .HasRequired(f => f.Owner)
                .WithMany(u => u.InfoSubscriptions)
                .Map(m => { m.MapKey("OwnerID"); });

            modelBuilder.Entity<InfoSubscription>()
                .HasRequired(f => f.Title)
                .WithMany(u => u.Subscriptions)
                .Map(m => { m.MapKey("TitleID"); });
        }
    }
}
