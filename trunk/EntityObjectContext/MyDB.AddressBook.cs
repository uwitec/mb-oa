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
        public DbSet<AddressBook> AddressBooks { get; set; }

        /// <summary>
        /// 事件共享表
        /// </summary>
        public DbSet<AddressBookShare> AddressBookShares { get; set; }

        private void AddressBookModelCreate(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressBook>()
                .HasRequired(e => e.Creator)
                .WithMany(u => u.CreateAddressBooks)
                .Map(m => { m.MapKey("Creator"); });

            modelBuilder.Entity<AddressBook>()
                .HasRequired(e => e.Owner)
                .WithMany(u => u.OwnAddressBooks)
                .Map(m => { m.MapKey("Owner"); });

            modelBuilder.Entity<AddressBook>()
                .HasMany(e => e.AddressBookShares)
                .WithOptional(u => u.AddressBook)
                .Map(m => { m.MapKey("AddressBookID"); });

            modelBuilder.Entity<AddressBookShare>()
                .HasOptional(e => e.Subject)
                .WithMany(u => u.AddressBookShares)
                .Map(m => { m.MapKey("SubjectID"); });
        }
    }
}
