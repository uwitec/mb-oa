using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Script.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Json;
using System.IO;

namespace TestProject1
{
    [TestClass]
    public class JSONTest
    {
        [TestMethod]
        public void DeserializeObject()
        {
            const string jsonString = "{x:123,x:132,y:true,z:'chw',array:[{a1:'x',a2:123},{a1:'x',a3:123}]}";  //同名参数，取最后一个的值

            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, object> y = (Dictionary<string, object>)jss.DeserializeObject(jsonString);

            var obj = new { x = 123, t = DateTime.Now, b = true, i = 123, f = 123.33, ar = new[] { new { x = 1, y = 3 }, new { x = 2, y = 4 } } };
            string str = jss.Serialize(obj);

            Dictionary<string, object> z = (Dictionary<string, object>)jss.DeserializeObject(str);

            //z["x"]
            //((object[])z["ar"])[0]

            return;
        }

        [TestMethod]
        public void Json2XML()
        {
            var a = new { x = 123, y = "asdf", z = DateTime.Now, array = new[] { new { a = 1, b = true }, new { a = 2, b = false } } };

            JavaScriptSerializer jss = new JavaScriptSerializer();



            XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(jss.Serialize(a)), XmlDictionaryReaderQuotas.Max);
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(reader);
        }

        //dynamic
        [TestMethod]
        public void TestJsonObject()
        {
            string jsonText = "{ xyz: 123, items: [ 10, 100, 1000 ] }";
            JsonObject jsonObject = (JsonObject)JsonValue.Parse(jsonText);

            JsonArray items = (JsonArray)jsonObject["items"];
            items[2] = 1001;

            JsonObject bar = new JsonObject();
            bar["name"] = "c#";
            jsonObject["bar"] = bar;

            StringWriter sw = new StringWriter();
            jsonObject.Save(sw);
        }

    }
}
