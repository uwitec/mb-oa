﻿using System;
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

        [TestMethod]
        public void TestCrossTable()
        {
            MyDB mydb = new MyDB();

            Organization[] orgs1 = mydb.Roles.SelectMany(r => r.Subjects).OfType<Organization>().ToArray();

            //Organization[] orgs2 = mydb.Roles.SelectMany(r => r.Organizations).ToArray();

            return;
        }

        [TestMethod]
        public void cast()
        {
            Subject s;
            MyDB mydb = new MyDB();
            {
                s = mydb.Subjects.First();
            }
            Convert.ChangeType(s, s.GetType());

            System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFrom("EntityObjectLib.dll");
            Type t = ass.GetType("EntityObjectLib.Subject");
            object so = Convert.ChangeType(s, t.GetType());
        }
    }
}
