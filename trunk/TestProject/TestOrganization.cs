using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityObjectContext;
using System.Diagnostics;
using EntityObjectLib;

namespace TestProject1
{
    [TestClass]
    public class TestOrganization
    {
        [TestMethod]
        public void TestMethod1()
        {
            MyDB mydb = new MyDB();

            OrganizationExt m = mydb.OrganizationExts.FirstOrDefault();
            Debug.WriteLine(m.Name);
        }

    }
}
