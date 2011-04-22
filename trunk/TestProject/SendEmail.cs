using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using System.Net;

namespace TestProject1
{
    [TestClass]
    public class SendEmail
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                MailMessage mailObj = new MailMessage();
                mailObj.From = new MailAddress("chen@manbuit.com");
                mailObj.To.Add("437968474@qq.com");
                mailObj.Subject = "主题";
                mailObj.Body = "内容";

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.exmail.qq.com";
                smtp.Port = 465;

                //smtp.UseDefaultCredentials = true;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("chen@manbuit.com", "971023chw");

                smtp.Send(mailObj);
            }
            catch
            {
            }
        }
    }
}
