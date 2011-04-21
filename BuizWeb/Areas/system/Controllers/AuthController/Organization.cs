using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using EntityObjectContext;
using System.Data.Entity;

namespace BuizApp.Areas.system.Controllers
{
    //[MyFilter]
    public partial class AuthController : Controller
    {
        public ActionResult getOrg()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Organization p = mydb.Organizations.Find(Request.Form["ID"]);
                return Json(new
                {
                    success = true,
                    data = new { p.ID,p.Code,p.Name,p.OrderNO }
                }
                );
            }
        }
        /// <summary>
        /// 新建操作
        /// 请求方式: ajax请求
        /// 请求参数: JSON
        ///     [ID,moduleCode,moduleName,moduleDescription]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOrg()
        {
            //将JSON格式转换为Module类型
            //return Json(new { success = false, errors = new { clientCode = "", portOfLoading = "" } });
            
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Organization p = getOrg(Request, mydb);
                p.ID = Guid.NewGuid().ToString();
                mydb.Organizations.Add(p);
                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateOrg()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Organization p = getOrg(Request, mydb);
                ////mydb.Modules.Attach(p);
                //mydb.Entry<EntityObjectLib.User>(p).State = System.Data.EntityState.Modified;
                //mydb.Entry<EntityObjectLib.Organization>(p.Organization).State = System.Data.EntityState.Modified;
                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteOrg()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Organization p = mydb.Organizations.Find(Request.Form["ID"]);
                mydb.Organizations.Remove(p);
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        private EntityObjectLib.Organization getOrg(HttpRequestBase request, MyDB mydb)
        {
            EntityObjectLib.Organization p = mydb.Organizations.Find(Request.Form["ID"]);
            if (p == null)
            {
                p = new EntityObjectLib.Organization();
            }
            p.ID = request.Form["ID"];
            p.Code = request.Form["Code"];
            p.Name = request.Form["Name"];
            return p;
        }

        [HttpPost]
        public ActionResult updateOrgRoles()
        {
            IEnumerable<string> Ids = Request.Params["IDs"].Split(",".ToArray()).AsEnumerable(); //新的角色ID串
            string orgID = Request.Params["orgID"];
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Organization org = mydb.Organizations.Find(orgID);

                IQueryable<string> OriRoleIDs = org.Roles.Select(r => r.ID) == null ? null : org.Roles.Select(r => r.ID).AsQueryable();

                OriRoleIDs.Load();

                string[] removeIDS = OriRoleIDs.Except(Ids).ToArray();
                foreach (string s in removeIDS)
                {
                    org.Roles.Remove(mydb.Roles.Find(s));
                }

                string[] appendIDS = Ids.Except(OriRoleIDs).ToArray();
                foreach (string s in appendIDS)
                {
                    org.Roles.Add(mydb.Roles.Find(s));
                }
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult updateOrgUsers()
        {
            IEnumerable<string> Ids = Request.Params["IDs"].Split(",".ToArray()).AsEnumerable(); //新的用户ID串
            string orgID = Request.Params["orgID"];
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Organization org = mydb.Organizations.Find(orgID);
                mydb.Users.Load();

                foreach (string userID in Ids)
                {
                    mydb.Users.Local.FirstOrDefault(u => u.ID.Equals(userID)).Organization = org;
                }
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }
    }
}
