using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using BuizApp.App_Code;
using System.Data.Entity;

namespace BuizApp.Areas.msgService.Controllers
{
    public class getOrgizationsController : Controller
    {
        //
        // GET: http://localhost:12480/msgService/getOrgizations?user=chw&pwd=123456

        public string Index()
        {
            string userCode = Request.QueryString["user"];
            string password = Request.QueryString["pwd"];

            Response.Clear();
            Response.ContentType = "text/xml";

            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.User user = mydb.Users.First(u => u.Code.ToLower() == userCode.ToLower() && u.Password == password);
                if (user != null)
                {
                    mydb.Organizations.Load();
                    object[] result = mydb.Organizations.Local.Where(o => o.Parent == null).Select(o => getOrg(o.ID, mydb)).ToArray();
                    return CJX.Object2XML(result).OuterXml;
                }
                else
                {
                    return "<?xml version=\"1.0\"?><root><error>用户名口令错误!</error></root>";
                }
            }
        }

        private object getOrg(string OrgID, MyDB mydb)
        {
            EntityObjectLib.Organization org = mydb.Organizations.Local.FirstOrDefault(o => o.ID.Equals(OrgID));
            return new
            {
                ID = OrgID,
                org.Name,
                org.Code,
                //expanded = true,
                leaf = org.Children.Count == 0/*org.Children.Count() == 0*/,
                //@checked = false,
                //iconCls = "icon-org",
                children = org.Children.Select(o => getOrg(o.ID, mydb)).ToArray()
            };
        }
    }
}
