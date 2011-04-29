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
            EventModelCreate(modelBuilder);
            AddressBookModelCreate(modelBuilder);
        }
    }
}
