using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Web;
using System.IO;

namespace TestProject1
{
    [TestClass]
    public class SMS
    {
        [TestMethod]
        public void TestMethod1()
        {
            SMSSend("18951163698", "这是一条测试信息");
        }

        private bool SMSSend( string mobile, string msg)
        {
            // 发送请求
            string requestBody = string.Format("{0}={1}&{2}={3}&{4}={5}&{6}={7}&{8}={9}&{10}={11}"
                , HttpUtility.UrlEncode("user_id", Encoding.GetEncoding("GB2312"))
                , HttpUtility.UrlEncode("3320", Encoding.GetEncoding("GB2312"))

                , HttpUtility.UrlEncode("password", Encoding.GetEncoding("GB2312"))
                , HttpUtility.UrlEncode("m8k7e1q5", Encoding.GetEncoding("GB2312"))

                , HttpUtility.UrlEncode("mobile_phone", Encoding.GetEncoding("GB2312"))
                , HttpUtility.UrlEncode(mobile, Encoding.GetEncoding("GB2312"))

                , HttpUtility.UrlEncode("msg", Encoding.GetEncoding("GB2312"))
                , HttpUtility.UrlEncode(msg, Encoding.GetEncoding("GB2312"))

                , HttpUtility.UrlEncode("sendtime", Encoding.GetEncoding("GB2312"))
                , HttpUtility.UrlEncode(string.Empty, Encoding.GetEncoding("GB2312"))

                , HttpUtility.UrlEncode("subcode", Encoding.GetEncoding("GB2312"))
                , HttpUtility.UrlEncode("3320", Encoding.GetEncoding("GB2312"))
                );

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://bms.hichina.com/sms_gateway/sms_api");
            request.Method = "POST";
            request.KeepAlive = false;
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] aryBuf = Encoding.GetEncoding("GB2312").GetBytes(requestBody);
            request.ContentLength = aryBuf.Length;
            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(aryBuf, 0, aryBuf.Length);
                writer.Close();
                writer.Dispose();
            }
            string ret = string.Empty;
            using (WebResponse response = request.GetResponse())
            {
                StreamReader reader = new StreamReader(response.GetResponseStream()
                    , Encoding.GetEncoding("GB2312")
                    );
                ret = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
            }
            if (ret == "0")
                return true;
            else
                return false;
        }

    }
}
