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
    public class MyDB : DbContext // MyDB是连接串的名称
    {
        // 权限
        public DbSet<Organization> Organizations { get; set; } //Categories是表名
        public DbSet<OrganizationExt> OrganizationExts { get; set; } //CategoriesExt是表名
        public DbSet<Module> Modules { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RolePrivilege> RolePrivileges { get; set; }

        // 消息管理
        public DbSet<Info> Infos { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<MyInfo> MyInfos { get; set; }

        public DbSet<InfoFile> InfoFiles { get; set; }

        public DbSet<InfoBoard> InfoBoards { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public MyDB()
            : base("MyDB2") // MyDB2是连接串的名称
        {
            this.Configuration.LazyLoadingEnabled = true; //启用延迟加载，未验证
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ValidateOnSaveEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            PrivilegeModelCreate(modelBuilder);
            InfoModelCreate(modelBuilder);
        }

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

            modelBuilder.Entity<MyInfo>().ToTable("MyInfos");
            modelBuilder.Entity<MyInfo>()
                .HasRequired(f => f.Info)
                .WithMany(i => i.Receivers)
                .Map(m => { m.MapKey("InfoID"); });

            modelBuilder.Entity<MyInfo>()
                .HasRequired(f => f.Receiver)
                .WithMany(u => u.ReceiveInfos)
                .Map(m => { m.MapKey("Receiver"); });

            modelBuilder.Entity<InfoBoard>()
                .HasRequired(b => b.Administrator)
                .WithMany(u => u.InfoBoards)
                .Map(m => { m.MapKey("Administrator"); });

            modelBuilder.Entity<Subscription>()
                .HasRequired(f => f.Owner)
                .WithMany(u => u.Subscriptions)
                .Map(m => { m.MapKey("OwnerID"); });

            modelBuilder.Entity<Subscription>()
                .HasRequired(f => f.Title)
                .WithMany(u => u.Subscriptions)
                .Map(m => { m.MapKey("TitleID"); });
        }

        private void PrivilegeModelCreate(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Privilege>()
                .HasRequired(p => p.resource) //必须在resourceID设置特性Required
                .WithMany(res => res.privileges)
                .Map(m => { m.MapKey("resourceID"); });// 在Privilege中不定义resourceID，用该语句建立关联，建议这样做

            modelBuilder.Entity<Resource>()
                .HasRequired(r => r.module) //必须在resourceID设置特性Required
                .WithMany(m => m.resources)
                .Map(m => { m.MapKey("moduleID"); });// 在Privilege中不定义resourceID，用该语句建立关联，建议这样做

            modelBuilder.Entity<RolePrivilege>()
                .HasRequired(rp => rp.Privilege)
                .WithMany(p => p.PrivilegeRoles)
                .Map(m => m.MapKey("PrivilegeID"));

            modelBuilder.Entity<RolePrivilege>()
                .HasRequired(rp => rp.Role)
                .WithMany(p => p.RolePrivileges)
                .Map(m => m.MapKey("RoleID"));

            modelBuilder.Entity<Role>()
                .HasMany(p => p.Subjects)
                .WithMany(t => t.Roles)
                .Map(m =>
                    {
                        m.ToTable("Subjects_Roles");
                        m.MapLeftKey("RoleID");
                        m.MapRightKey("SubjectID");
                    });

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Organization)
                .WithMany(o => o.Users)
                .Map(m => { m.MapKey("OrgID"); });
            modelBuilder.Entity<User>().ToTable("Users");//以多表方式实现继承Table-per-Hierarchy

            modelBuilder.Entity<Organization>()
                .HasOptional(c => c.Parent)
                .WithMany(c => c.Children)
                .Map(m => m.MapKey("ParentID"));
            modelBuilder.Entity<Organization>().ToTable("Organizations");//Table-per-Hierarchy
            modelBuilder.Entity<OrganizationExt>().ToTable("OrganizationExts");//以多表方式实现继承
        }
    }
}
