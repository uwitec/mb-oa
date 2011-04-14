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
        public DbSet<Organization> Organizations { get; set; } //Categories是表名
        public DbSet<OrganizationExt> OrganizationExts { get; set; } //CategoriesExt是表名
        public DbSet<Module> Modules { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RolePrivilege> RolePrivileges { get; set; }

        public MyDB()
            : base("MyDB2") // MyDB2是连接串的名称
        {
            this.Configuration.LazyLoadingEnabled = true; //启用延迟加载，未验证
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ValidateOnSaveEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region 测试
            modelBuilder.Entity<Organization>()
                .HasOptional(c => c.ParentOrganization)
                .WithMany(c => c.ChildOrganizations)
                .Map(m => m.MapKey("ParentID"));

            modelBuilder.Entity<OrganizationExt>().ToTable("OrganizationExts");//以多表方式实现继承
            #endregion

            // 以下用module的[Association("Resource_module", "moduleID", "ID", IsForeignKey = true)]，moduleID必须存在
            //modelBuilder.Entity<Resource>()
            //    .HasOptional(res => res.module)
            //    .WithMany(m => m.resources)
            //    .HasForeignKey(res => res.moduleID);

            modelBuilder.Entity<Privilege>()
                .HasRequired(p => p.resource) //必须在resourceID设置特性Required
                .WithMany(res => res.privileges)
                .Map(m => { m.MapKey("resourceID"); });// 在Privilege中不定义resourceID，用该语句建立关联，建议这样做
            //.HasForeignKey(p => p.resourceID); // 如果在Privilege中定义了resourceID，可以用该语句建立关联，但建议不这样做

            modelBuilder.Entity<Role>()
                .HasMany(p => p.Subjects)
                .WithMany(t => t.Roles)
                .Map(m =>
                {
                    m.ToTable("Subjects_Roles");
                    m.MapLeftKey("RoleID");
                    m.MapRightKey("SubjectID");
                    //m.MapLeftKey(p => p.ID, "RoleID");  // CTP5,已淘汰
                    //m.MapRightKey(t => t.ID, "UserID");  // CTP5,已淘汰
                });

            //modelBuilder.Entity<Role>()
            //    .HasMany(r => r.Users)
            //    .WithMany(u => u.Roles)
            //    .Map(m =>
            //    {
            //        m.ToTable("Subjects_Roles");
            //        m.MapLeftKey("RoleID");
            //        m.MapRightKey("SubjectID");
            //        //m.MapLeftKey(p => p.ID, "RoleID");  // CTP5,已淘汰
            //        //m.MapRightKey(t => t.ID, "UserID");  // CTP5,已淘汰
            //    });

            modelBuilder.Entity<User>().ToTable("Users");//以多表方式实现继承Table-per-Hierarchy

            modelBuilder.Entity<User>()
                .HasOptional(u => u.Organization)
                .WithMany(o => o.Users)
                .Map(m => { m.MapKey("OrgID"); });

            modelBuilder.Entity<Organization>().ToTable("Organizations");//Table-per-Hierarchy

            modelBuilder.Entity<RolePrivilege>()
                .HasRequired(rp => rp.Privilege)
                .WithMany(p => p.PrivilegeRoles)
                .Map(m => m.MapKey("PrivilegeID"));

            modelBuilder.Entity<RolePrivilege>()
                .HasRequired(rp => rp.Role)
                .WithMany(p => p.RolePrivileges)
                .Map(m => m.MapKey("RoleID"));
        }
    }
}
