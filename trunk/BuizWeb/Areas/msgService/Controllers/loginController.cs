using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using BuizApp.App_Code;

namespace BuizApp.Areas.msgService.Controllers
{
    public class loginController : Controller
    {
        /// <summary>
        /// 验证用户名和口令,例如:http://localhost:12480/msgService/login?user=chw&pwd=123456
        /// </summary>
        /// <returns></returns>
        public string Index()
        {
            string userCode = Request.QueryString["user"];
            string password = Request.QueryString["pwd"];

            Response.Clear();
            Response.ContentType = "text/xml";

            bool result = false;
            using (MyDB mydb = new MyDB())
            {
                result = mydb.Users.Count(u => u.Code.ToLower() == userCode.ToLower() && u.Password == password) > 0;

                return CJX.Object2XML(new { result = result }).OuterXml;
            }
        }
    }
}
