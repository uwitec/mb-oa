using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityObjectContext;

namespace TestProject1
{
    [TestClass]
    public class EFLinq
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (MyDB mydb = new MyDB())
            {
                var s = mydb.Privileges.GroupJoin(
                    mydb.RolePrivileges.Where(rp => rp.Role.roleCode == "normal user"),
                    p => p.ID,
                    rp => rp.Privilege.ID,
                    (p, rp) => new { p.ID, p.privilegeCode, p.privilegeName, p.orderNO, has = rp.Count() > 0 }
                        );
                return;
            }
        }
    }
}
