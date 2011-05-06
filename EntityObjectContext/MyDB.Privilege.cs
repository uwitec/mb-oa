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
        public DbSet<UserGroup> UserGroups { get; set; }

        private void PrivilegeModelCreate(DbModelBuilder modelBuilder)
        {
            // 权限资源关系
            modelBuilder.Entity<Privilege>()
                .HasRequired(p => p.resource) //必须在resourceID设置特性Required
                .WithMany(res => res.privileges)
                .Map(m => { m.MapKey("resourceID"); });// 在Privilege中不定义resourceID，用该语句建立关联，建议这样做

            // 资源模块关系
            modelBuilder.Entity<Resource>()
                .HasRequired(r => r.module) //必须在resourceID设置特性Required
                .WithMany(m => m.resources)
                .Map(m => { m.MapKey("moduleID"); });// 在Privilege中不定义resourceID，用该语句建立关联，建议这样做

            //  角色权限与权限的关系
            modelBuilder.Entity<RolePrivilege>()
                .HasRequired(rp => rp.Privilege)
                .WithMany(p => p.PrivilegeRoles)
                .Map(m => m.MapKey("PrivilegeID"));

            // 角色权限与角色的关系
            modelBuilder.Entity<RolePrivilege>()
                .HasRequired(rp => rp.Role)
                .WithMany(p => p.RolePrivileges)
                .Map(m => m.MapKey("RoleID"));

            // 角色主体关系
            modelBuilder.Entity<Role>()
                .HasMany(p => p.Subjects)
                .WithMany(t => t.Roles)
                .Map(m =>
                    {
                        m.ToTable("Subjects_Roles");
                        m.MapLeftKey("RoleID");
                        m.MapRightKey("SubjectID");
                    });

            // 用户所在组织
            modelBuilder.Entity<User>()
                .HasOptional(u => u.Organization)
                .WithMany(o => o.Users)
                .Map(m => { m.MapKey("OrgID"); });
            modelBuilder.Entity<User>().ToTable("Users");//以多表方式实现继承Table-per-Hierarchy

            // 组织的树结构
            modelBuilder.Entity<Organization>()
                .HasOptional(c => c.Parent)
                .WithMany(c => c.Children)
                .Map(m => m.MapKey("Parent"));

            // 组织派生于主体,在此处声明是表明使用单表继承
            modelBuilder.Entity<Organization>().ToTable("Organizations");//Table-per-Hierarchy

            // 组织扩展派生于组织,在此处声明是表明使用单表继承,暂未使用,以后可用于划分分公司和部门
            modelBuilder.Entity<OrganizationExt>().ToTable("OrganizationExts");//以多表方式实现继承

            // 组织管理者关联,管理者可以不在所管理的组织中
            modelBuilder.Entity<Organization>()
                .HasOptional(org => org.Manager)
                .WithMany(u => u.ManageOrgs)
                .Map(m => m.MapKey("Manager"));

            // 用户组用户关联
            modelBuilder.Entity<UserGroup>()
                .HasMany(p => p.Users)
                .WithMany(t => t.UserGroups)
                .Map(m =>
                {
                    m.ToTable("Users_UserGroups");
                    m.MapLeftKey("UserGroupID");
                    m.MapRightKey("UserID");
                });

            // 用户组派生于主体,在此处声明是表明使用单表继承
            modelBuilder.Entity<UserGroup>().ToTable("UserGroups");//以多表方式实现继承Table-per-Hierarchy
        }
    }
}
