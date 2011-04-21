using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityObjectLib;
using EntityObjectContext;

namespace BuizModel
{
    public class PrivilegeModel
    {
        public static object[] getList()
        {
            using (MyDB mydb = new MyDB())
            {
                return mydb.Privileges
                    .Select(p => new { p.ID, p.resource.resourceName, p.resource.module.moduleName, p.privilegeName, p.privilegeCode, p.needAuth, p.isMenuEntry, p.createdTime, p.orderNO }) //快速匿名对象
                    //.Select(p => p) //是否可以通过判断类型来取属性，例如只取简单类型的属性
                    .ToArray();
            }
        }

        public static object[] getListByModule(string ModuleID)
        {
            using (MyDB mydb = new MyDB())
            {
                return mydb.Privileges.Where(p => p.resource.module.ID.Equals(ModuleID))
                    .OrderBy(p => p.resource.module.orderNO)
                    .OrderBy(p => p.resource.orderNO)
                    .OrderBy(p => p.orderNO)
                    .Select(p => new { p.ID, p.resource.resourceName, p.resource.module.moduleName, p.privilegeName, p.privilegeCode, p.needAuth, p.isMenuEntry, p.createdTime, p.orderNO }) //快速匿名对象
                    .ToArray();
            }
        }

        public static object[] getListByResource(string ResourceID)
        {
            using (MyDB mydb = new MyDB())
            {
                return mydb.Privileges.Where(p => p.resource.ID.Equals(ResourceID))
                    .Select(p => new { p.ID, p.resource.resourceName, p.resource.module.moduleName, p.privilegeName, p.privilegeCode, p.needAuth, p.isMenuEntry, p.createdTime, p.orderNO }) //快速匿名对象
                    //.Select(p => p) //是否可以通过判断类型来取属性，例如只取简单类型的属性
                    .ToArray();
            }
        }
        public static object[] getModuleResourceTreeData()
        {
            using (MyDB mydb = new MyDB())
            {
                return mydb.Modules.OrderBy(m => m.orderNO).Select(
                        m => new
                        {
                            id = m.ID,
                            text = m.moduleName + "[" + m.moduleCode + "]",
                            type = "module",
                            expanded = true,
                            @checked = true,
                            children = m.resources.OrderBy(r => r.orderNO).Select(
                                r => new
                                {
                                    id = r.ID,
                                    text = r.resourceName + "[" + r.resourceCode + "]",
                                    type = "resource",
                                    leaf = true,
                                    @checked = true
                                }
                            )
                        }
                    ).ToArray();
            }
        }

        public static object[] rolePrivilege(string roleId)
        {
            // 需要使用左外连接,导出所有权限,以及每个权限是否赋给该角色
            using (MyDB mydb = new MyDB())
            {
                //return mydb.RolePrivileges.Where(rp => rp.Role.ID.Equals(roleId))
                //        .Select(
                //        rp => new
                //        {
                //            rp.ID,
                //            privilegeID = rp.Privilege.ID,
                //            rp.Privilege.privilegeCode,
                //            rp.Privilege.privilegeName,
                //            rp.Parameters
                //        }).ToArray();
                return
                    mydb.Privileges
                        .OrderBy(p=>p.resource.module.orderNO).OrderBy(p=>p.resource.orderNO).OrderBy(p=>p.orderNO)
                        .GroupJoin(
                        mydb.RolePrivileges.Where(rp => rp.Role.ID.Equals(roleId)),
                        p => p.ID,
                        rp => rp.Privilege.ID,
                        (p, rp) => new { privilegeID = p.ID, p.privilegeCode, p.privilegeName, p.resource.resourceName, p.resource.module.moduleName, p.orderNO, ID = rp.FirstOrDefault() != null ? rp.FirstOrDefault().ID : null, Parameters = rp.FirstOrDefault() != null ? rp.FirstOrDefault().Parameters : null, @checked = rp.Count() > 0 }
                        ).ToArray();
            }
        }

        public static object[] getModuleResourceTreeData2()
        {
            using (MyDB mydb = new MyDB())
            {
                return mydb.Modules.OrderBy(m => m.orderNO).Select(
                        m => new
                        {
                            id = m.ID,
                            text = m.moduleName + "[" + m.moduleCode + "]",
                            type = "module",
                            expanded = true,
                            //@checked = true,//checked是C#关键字
                            children = m.resources.OrderBy(r => r.orderNO).Select(
                                r => new
                                {
                                    id = r.ID,
                                    text = r.resourceName + "[" + r.resourceCode + "]",
                                    type = "resource",
                                    expanded = true,
                                    children = r.privileges.OrderBy(p => p.orderNO).Select(
                                        p => new
                                        {
                                            id = p.ID,
                                            text = p.privilegeName + "[" + p.privilegeCode + "]",
                                            type = "privilege",
                                            @checked = true,
                                            leaf = true,
                                        }
                                        )
                                }
                            )
                        }
                    ).ToArray();
            }
        }
    }
}
