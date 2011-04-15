using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Script.Serialization;

namespace TestProject1
{
    public class dd
    {
        public int x;
        public string y;
    }
    [TestClass]
    public class TestJSON
    {
        [TestMethod]
        public void TestMethod1()
        {
            string jsonstr = "{x:11,y:'abcd'}";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            object obj = serializer.DeserializeObject(jsonstr);
            Dictionary<string, object> dic = serializer.DeserializeObject(jsonstr) as Dictionary<string, object>;
            dd d = serializer.Deserialize<dd>(jsonstr);
        }
    }
}
