using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using System.Xml;
using System.Reflection;
using BuizApp.App_Code;

namespace BuizApp.Areas.msgService.Controllers
{
    public class getUsersController : Controller
    {
        //
        // GET: http://localhost:12480/msgService/getUsers?user=chw&pwd=123456

        public string Index()
        {
            string userCode = Request.QueryString["user"];
            string password = Request.QueryString["pwd"];

            Response.Clear();
            Response.ContentType = "text/xml";

            using (MyDB mydb = new MyDB())
            {
                bool check = mydb.Users.Count(u => u.Code.ToLower() == userCode.ToLower() && u.Password == password) > 0;
                if (check)
                {
                    //xmlString = "<?xml version=\"1.0\"?><Root>" +
                    //    string.Join("\r\n",
                    //        mydb.Users
                    //        .OrderBy(u => u.OrderNO)
                    //        .Select(u => "<User><ID>" + u.ID + "</ID><Code>" + u.Code + "</Code><Name>" + u.Name + "</Name></User>").ToArray()
                    //    )
                    //    + "</Root>";
                    //BuizApp.App_Code.CJX.Json2XML("asdf");
                    return CJX.Object2XML(
                        mydb.Users
                        .OrderBy(u => u.OrderNO)
                        .Select(u=>new {u.ID,u.Code,u.Name})
                        .ToArray()
                        ).OuterXml;
                }
                else
                {
                    return "<?xml version=\"1.0\"?><root><error>用户名口令错误!</error></root>";
                }
            }

            //string xmlString = string.Format("<Root><userCode>{0}</userCode><password>{1}</password></Root>", userCode, password);
        }
    }
}
