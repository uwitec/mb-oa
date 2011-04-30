using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityObjectContext;
using EntityObjectLib;
using System.Data.Entity;

namespace TestProject1
{
    [TestClass]
    public class AddressBookTest
    {
        [TestMethod]
        public void Append()
        {
            using (MyDB mydb = new MyDB())
            {
                #region "创建通讯录记录"
                AddressBook AddressBook = new AddressBook
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "陈宏伟",
                    Sex = "男",
                    BirthDay = new DateTime(1971, 9, 10),
                    Company = "manbuit",
                    Department = "R&D",
                    Job = "developer",
                    Creator = mydb.Users.First(s => s.Code.Equals("lilin")),
                    Owner = mydb.Users.First(s => s.Code.Equals("lilin")),
                    AddressBookShares = new AddressBookShare[] {
                        new AddressBookShare
                        {
                            ID = Guid.NewGuid().ToString(),
                            Subject = mydb.Subjects.OfType<User>().First(s=>s.Code.Equals("lilin"))
                        },                        
                        new AddressBookShare
                        {
                            ID = Guid.NewGuid().ToString(),
                            Subject = mydb.Subjects.OfType<User>().First(s=>s.Code.Equals("chw"))
                        }
                    }
                };
                #endregion

                mydb.AddressBooks.Add(AddressBook);
                mydb.SaveChanges();
            }
        }

        [TestMethod]
        public void clear()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.AddressBookShares.Load();
                mydb.AddressBooks.Load();

                mydb.AddressBookShares.Local.Clear();
                mydb.AddressBooks.Local.Clear();

                mydb.SaveChanges();
            }
        }
    }
}
