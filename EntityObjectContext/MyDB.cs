﻿using System;
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
            this.PrivilegeModelCreate(modelBuilder);
            this.InfoModelCreate(modelBuilder);
            this.EventModelCreate(modelBuilder);
            this.AddressBookModelCreate(modelBuilder);
            this.WFDefineModelCreate(modelBuilder);
            this.WFInstModelCreate(modelBuilder);
            this.GenericBuizModelCreate(modelBuilder);
            this.FWModelCreate(modelBuilder);
            //this.ApplyExpenseModelCreate(modelBuilder);
        }
    }
}
