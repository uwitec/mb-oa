using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using System.Web.Script.Serialization;
using BuizModel;
using EntityObjectContext;
using System.Data.Entity;

namespace BuizApp.Areas.data.Controllers
{
    public class PrivilegeController : Controller
    {
        public JsonResult index()
        {
            string mrId = Request.Params["mrId"];
            string depth = Request.Params["depth"];
            switch (depth)
            {
                case "1": //模块
                    return Json(new { success = true, data = PrivilegeModel.getListByModule(mrId) }, JsonRequestBehavior.AllowGet);
                case "2": //功能
                    return Json(new { success = true, data = PrivilegeModel.getListByResource(mrId) }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { success = true, data = PrivilegeModel.getList() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult rolePrivilege()
        {
            return Json(PrivilegeModel.rolePrivilege(Request.Params["roleID"]), JsonRequestBehavior.AllowGet);
        }

        public JsonResult moduleResourceTree()
        {
            return Json(new { text = "ALL", children = PrivilegeModel.getModuleResourceTreeData() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult moduleResourceTree2()
        {
            return Json(new { text = "ALL", children = PrivilegeModel.getModuleResourceTreeData2() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult role()
        {
            using (MyDB mydb = new MyDB())
            {
                return Json(
                    mydb.Roles.Select(
                        r => new
                        {
                            r.ID,
                            r.roleCode,
                            r.roleName,
                            r.roleDescription
                        }).ToArray(),
                    JsonRequestBehavior.AllowGet); ;
            }
        }

        public JsonResult roleByUser()
        {
            string userID = Request.Params["userID"];
            using (MyDB mydb = new MyDB())
            {
                object[] userRoles =
                    mydb.Roles.GroupJoin(
                        mydb.Users.Find(userID).Roles.Select(r => r.ID)
                        , r => r.ID
                        , ru => ru
                        , (r, ru) => new { r.ID, r.roleCode, r.roleName, r.roleDescription, @checked = ru.Count() > 0, userID = userID }
                    ).ToArray();

                // 下面报错
                //    Unable to create a constant value of type 'EntityLib.Role '. Only primitive types ('such as Int32, String, and Guid') are supported in this context.
                //object[] userRoles =
                //    mydb.Roles.GroupJoin(
                //        mydb.Users.Find(userID).Roles //是这句上的问题，对比PrivilegeModel的rolePrivilege
                //        , r => r.ID
                //        , ru => ru.ID
                //        , (r, ru) => new { r.ID, r.roleCode, r.roleName, r.roleDescription, @checked = ru.Count() > 0, userID = userID }
                //    ).ToArray();

                return Json(userRoles, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult roleByOrg()
        {
            string orgID = Request.Params["orgID"];
            using (MyDB mydb = new MyDB())
            {
                object[] orgRoles =
                    mydb.Roles.GroupJoin(
                        mydb.Organizations.Find(orgID).Roles.Select(r => r.ID)
                        , r => r.ID
                        , ru => ru
                        , (r, ru) => new { r.ID, r.roleCode, r.roleName, r.roleDescription, @checked = ru.Count() > 0, orgID = orgID }
                    ).ToArray();

                return Json(orgRoles, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult userByOrg()
        {
            string orgID = Request.Params["orgID"];
            using (MyDB mydb = new MyDB())
            {
                object[] orgUsers =
                    mydb.Users.GroupJoin(
                        mydb.Organizations.Find(orgID).Users.Select(r => r.ID)
                        , r => r.ID
                        , ru => ru
                        , (r, ru) => new { r.ID, r.Code, r.Name, OrgName = r.Organization.Name, @checked = ru.Count() > 0, orgID = orgID }
                    ).ToArray();

                return Json(orgUsers, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult resource()
        {
            using (MyDB mydb = new MyDB())
            {
                return
                Json(
                            mydb.Resources.OrderBy(r=>r.orderNO).Select(
                            r => new
                            {
                                r.ID,
                                resourceName = r.module.moduleName+":"+r.resourceName
                            }).ToArray()
                    ,
                    JsonRequestBehavior.AllowGet
                    );
            }
        }

        public JsonResult module()
        {
            using (MyDB mydb = new MyDB())
            {
                return
                Json(mydb.Modules.OrderBy(r => r.orderNO).Select(r => new { r.ID, r.moduleName }).ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult user()
        {
            using (MyDB mydb = new MyDB())
            {
                return
                Json(mydb.Users.OrderBy(u => u.Code).Select(u => new { u.ID, u.Code, u.Name, u.Password, OrgID = u.Organization.ID, Organization=u.Organization.Name }).ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Organization()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.Organizations.Load();
                return Json(
                mydb.Organizations.Local.Select(o => new { o.ID,o.Code,o.Name,o.OrderNO}).OrderBy(org=>org.OrderNO).ToArray()
                , JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult OrganizationTree()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.Organizations.Load();
                object[] result = mydb.Organizations.Local.Where(o => o.Parent == null).Select(o => getOrg(o.ID, mydb)).ToArray();
                return Json(new { text = ",", children = result }, JsonRequestBehavior.AllowGet);
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
                expanded = true,
                leaf = org.Children.Count == 0/*org.Children.Count() == 0*/,
                //@checked = false,
                //iconCls = "icon-org",
                children = org.Children.Select(o => getOrg(o.ID, mydb)).ToArray()
            };
        }

        /// <summary>
        /// 返回组织用户树型结构
        /// </summary>
        /// <returns></returns>
        public JsonResult OrgUser()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.Organizations.Load();
                object[] result = mydb.Organizations.Local.Where(o => o.Parent==null).Select(o => getOrgAndUser(o.ID, mydb)).ToArray();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
           
        }

        private object getOrgAndUser(string OrgID, MyDB mydb)
        {
            EntityObjectLib.Organization org = mydb.Organizations.Local.FirstOrDefault(o=>o.ID.Equals(OrgID));
            return new
            {
                id = OrgID,
                text = org.Name,
                expanded = true,
                leaf = org.Users.Count == 0&&org.Children.Count==0/*org.Children.Count() == 0*/,
                //@checked = false,
                //iconCls = "icon-org",
                children =
                    org.Children.Select(o => getOrgAndUser(o.ID, mydb))
                    .Union(org.Users.Select(u => new { id = u.Code, text = u.Name, leaf = true, iconCls = "icon-user", @checked = false }))
                    .ToArray()
            };
        }

        #region 私有方法
        private string getJsonString(object o)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //jss.RecursionLimit = 1;
            return serializer.Serialize(o);
        }
        #endregion
    }
}
