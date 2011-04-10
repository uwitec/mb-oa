using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Diagnostics;

namespace TestProject1
{
    [TestClass]
    public class UnitTestForT4
    {
        const string path = @"C:\FromCVS\BuizApp\TestProject\metaData.xml";
        [TestMethod]
        public void test()
        {
            metaConfig mc = new metaConfig(path);
            Debug.WriteLine(mc.getCSharpCode());
        }


    }
}
