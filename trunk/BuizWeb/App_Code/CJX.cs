using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;

namespace BuizApp.App_Code
{
    public class CJX
    {
        public static XmlDocument Json2XML(string JsonString)
        {
            XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(JsonString), XmlDictionaryReaderQuotas.Max);
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(reader);
            return xdoc;
        }

        public static string Object2Json(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(obj);
        }

        public static XmlDocument Object2XML(object obj)
        {
            return Json2XML(Object2Json(obj));
        }
    }
}