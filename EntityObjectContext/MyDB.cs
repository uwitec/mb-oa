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
        public DbSet<Movie> Movies { get; set; } //Movies是表名
        public DbSet<Category> Categories { get; set; } //Categories是表名
        public DbSet<CategoryExt> CategoriesExt { get; set; } //CategoriesExt是表名
        public DbSet<Module> Modules { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

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
            // TODO: Use Fluent API Here
            modelBuilder.Entity<Movie>().HasKey(m => m.MPK); // 指明MPK为Movies的主键,等同于[Key]

            //modelBuilder.Entity<Category>().HasKey(c => c.Code); // 指明Code为Categories的主键,等同于[Key]

            //modelBuilder.Entity<Movie>().Property(m => m.CategoryCode).IsRequired(); //等同于[Required(ErrorMessage = "")]
            modelBuilder.Entity<Movie>() //指明Movie.CategoryCode为Category的外键
                //.HasOptional(m => m.MCategory) //可以不指定CategoryCode为必需
                .HasRequired(m => m.MCategory) //必须指定CategoryCode为必需
                .WithMany(c => c.Movies)
                .HasForeignKey(p => p.CategoryCode);

            modelBuilder.Entity<Category>()
                .HasOptional(c => c.ParentCategory)
                .WithMany(c => c.ChildCategories)
                .HasForeignKey(c => c.Parent);

            modelBuilder.Entity<CategoryExt>().ToTable("CategoriesExt");
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
                .HasMany(p => p.Privileges)
                .WithMany(t => t.Roles)
                .Map(m =>
                {
                    m.ToTable("Roles_Privileges");
                    m.MapLeftKey("RoleID");
                    m.MapRightKey("PrivilegeID");
                    //m.MapLeftKey(p => p.ID, "RoleID");  // CTP5,已淘汰
                    //m.MapRightKey(t => t.ID, "PrivilegeID");  // CTP5,已淘汰
                });

            modelBuilder.Entity<Role>()
                .HasMany(p => p.Users)
                .WithMany(t => t.Roles)
                .Map(m =>
                {
                    m.ToTable("Users_Roles");
                    m.MapLeftKey("RoleID");
                    m.MapRightKey("UserID");
                    //m.MapLeftKey(p => p.ID, "RoleID");  // CTP5,已淘汰
                    //m.MapRightKey(t => t.ID, "UserID");  // CTP5,已淘汰
                });
        }
    }
}
