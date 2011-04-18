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
        public ActionResult getRole()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Role p = mydb.Roles.Find(Request.Form["ID"]);
                return Json(new
                {
                    success = true,
                    data = new { p.ID, p.roleCode,p.roleName,p.roleDescription}
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
        public ActionResult CreateRole()
        {
            //将JSON格式转换为Module类型
            //return Json(new { success = false, errors = new { clientCode = "", portOfLoading = "" } });
            EntityObjectLib.Role p = getRole(Request);
            using (MyDB mydb = new MyDB())
            {
                p.ID = Guid.NewGuid().ToString();
                mydb.Roles.Add(p);
                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateRole()
        {
            EntityObjectLib.Role p = getRole(Request);
            using (MyDB mydb = new MyDB())
            {
                //mydb.Modules.Attach(p);
                mydb.Entry<EntityObjectLib.Role>(p).State = System.Data.EntityState.Modified;
                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRole()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Role p = mydb.Roles.Find(Request.Form["ID"]);
                mydb.Roles.Remove(p);
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        /// <summary>
        /// 对操作重新排序
        /// 使用本地实体集，参考：http://blogs.msdn.com/b/adonet/archive/2011/02/01/using-dbcontext-in-ef-feature-ctp5-part-7-local-data.aspx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReOrderRole()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.Roles.Load();
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        private EntityObjectLib.Role getRole(HttpRequestBase request)
        {
            EntityObjectLib.Role p = new EntityObjectLib.Role();
            p.ID = Request.Form["ID"];
            p.roleCode = Request.Form["roleCode"];
            p.roleName = Request.Form["roleName"];
            p.roleDescription = Request.Form["roleDescription"];
            return p;
        }

        [HttpPost]
        public ActionResult updateRolePrivivlege()
        {
            IEnumerable<string> Ids = Request.Params["IDs"].Split(",".ToArray()).AsEnumerable();
            string roleID = Request.Params["roleID"];
            using (MyDB mydb = new MyDB())
            {
                IQueryable<string> OriPrivilegeIDs =
                    mydb.RolePrivileges
                    .Where(rp => rp.Role.ID.Equals(roleID))
                    .Select(rp => rp.Privilege.ID);

                OriPrivilegeIDs.Load();

                foreach (string s in OriPrivilegeIDs)
                {
                    if (!Ids.Contains(s))
                    {
                        EntityObjectLib.RolePrivilege p = mydb.RolePrivileges
                            .FirstOrDefault(rp => rp.Role.ID.Equals(roleID) && rp.Privilege.ID.Equals(s));
                        mydb.RolePrivileges.Remove(p);
                    }
                }

                IEnumerable<string> preAppendPrivilegeIDs =
                    Ids.Except(
                    mydb.RolePrivileges
                    .Where(rp => rp.Role.ID.Equals(roleID))
                    .Select(rp => rp.Privilege.ID).ToArray()
                    );

                EntityObjectLib.Role role = mydb.Roles.Find(roleID);
                foreach (string s in preAppendPrivilegeIDs)
                {
                    EntityObjectLib.RolePrivilege rp = new EntityObjectLib.RolePrivilege();
                    rp.ID = Guid.NewGuid().ToString();
                    rp.Role = role;
                    rp.Privilege = mydb.Privileges.Find(s);
                    mydb.RolePrivileges.Add(rp);
                }
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult getRolePrivilegeParam()
        {
            string id = Request.Form["id"];
            using (MyDB mydb = new MyDB())
            {
                string Parameters = mydb.RolePrivileges.Find(id).Parameters;
                return Json(new
                        {
                            success = true,
                            data = new { Parameters }
                        }
                    );
            }
        }

        [HttpPost]
        public ActionResult updateRolePrivilegeParam()
        {
            string id = Request.Form["ID"];
            string Parameters = Request.Form["Parameters"];
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.RolePrivilege rp = mydb.RolePrivileges.Find(id);
                rp.Parameters = Parameters;
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }
    }
}
