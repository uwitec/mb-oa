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
        public ActionResult getUser()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.User p = mydb.Users.Find(Request.Form["ID"]);
                return Json(new
                {
                    success = true,
                    data = new { p.ID, p.Code, p.Name, p.Password, mobile = p.Mobile, p.MSN, p.QQ, p.OfficePhone, p.HomePhone, p.Email, p.ExpireDate, p.Description, OrgID = p.Organization.ID, Orgname = p.Organization.Name }
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
        public ActionResult CreateUser()
        {
            //将JSON格式转换为Module类型
            //return Json(new { success = false, errors = new { clientCode = "", portOfLoading = "" } });
            
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.User p = getUser(Request, mydb);
                p.ID = Guid.NewGuid().ToString();
                mydb.Users.Add(p);
                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateUser()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.User p = getUser(Request, mydb);
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
        public ActionResult DeleteUser()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.User p = mydb.Users.Find(Request.Form["ID"]);
                mydb.Users.Remove(p);
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        private EntityObjectLib.User getUser(HttpRequestBase request,MyDB mydb)
        {
            EntityObjectLib.User p = mydb.Users.Find(Request.Form["ID"]);
            if (p == null)
            {
                p = new EntityObjectLib.User();
            }
            p.ID = request.Form["ID"];
            p.Code = request.Form["Code"];
            p.Name = request.Form["Name"];
            p.Password = request.Form["Password"];
            p.Email = request.Form["Email"];
            p.Mobile = request.Form["Mobile"];
            p.MSN = request.Form["MSN"];
            p.QQ = request.Form["QQ"];
            p.OfficePhone = request.Form["OfficePhone"];
            p.HomePhone = request.Form["HomePhone"];
            p.ExpireDate = Convert.ToDateTime(request.Form["ExpireDate"]);
            p.Description = request.Form["Description"];
            p.Organization = mydb.Organizations.Find(request.Form["OrgID"]);
            return p;
        }

        [HttpPost]
        public ActionResult updateUserRoles()
        {
            IEnumerable<string> Ids = Request.Params["IDs"].Split(",".ToArray()).AsEnumerable(); //新的角色ID串
            string userID = Request.Params["userID"];
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.User user = mydb.Users.Find(userID);

                IQueryable<string> OriRoleIDs = user.Roles.Select(r => r.ID).AsQueryable();

                OriRoleIDs.Load();

                string[] removeIDS = OriRoleIDs.Except(Ids).ToArray();
                foreach (string s in removeIDS)
                {
                    user.Roles.Remove(mydb.Roles.Find(s));
                }

                string[] appendIDS = Ids.Except(OriRoleIDs).ToArray();
                foreach (string s in appendIDS)
                {
                    user.Roles.Add(mydb.Roles.Find(s));
                }
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }
    }
}
