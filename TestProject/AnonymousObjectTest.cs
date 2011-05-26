using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Web.Script.Serialization;

namespace TestProject1
{
    [TestClass]
    public class AnonymousObjectTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = new { Code = "code", Name = "chw" };

            Type t = a.GetType();

            JavaScriptSerializer jss = new JavaScriptSerializer();

            foreach (PropertyInfo pi in t.GetProperties())
            {
            }
        }
    }
}
