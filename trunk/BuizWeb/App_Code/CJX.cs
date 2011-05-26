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
        public static XmlDocument JSON2XML(string JsonString)
        {
            XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(JsonString), XmlDictionaryReaderQuotas.Max);
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(reader);
            return xdoc;
        }

        public static string Object2JSON(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(obj);
        }

        public static XmlDocument Object2XML(object obj)
        {
            return JSON2XML(Object2JSON(obj));
        }

        public static object JSON2Object(string JSONString)
        {
            
            throw (new NotImplementedException());
        }

        public static object XML2Object(XmlDocument doc)
        {
            throw (new NotImplementedException());
        }

        public static string XML2JSON(XmlDocument doc)
        {
            throw (new NotImplementedException());
        }
    }
}