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
                    .Select(p => new { p.ID, p.resource.resourceName,p.resource.module.moduleName,p.privilegeName, p.privilegeCode,p.needAuth,p.isMenuEntry,p.createdTime, p.orderNO }) //快速匿名对象
                    //.Select(p => p) //是否可以通过判断类型来取属性，例如只取简单类型的属性
                    .ToArray();
            }
        }

        public static object[] getListByModule(string ModuleID)
        {
            using (MyDB mydb = new MyDB())
            {
                return mydb.Privileges.Where(p=>p.resource.module.ID.Equals(ModuleID))
                    .Select(p => new { p.ID, p.resource.resourceName, p.resource.module.moduleName, p.privilegeName, p.privilegeCode, p.needAuth, p.isMenuEntry, p.createdTime, p.orderNO }) //快速匿名对象
                    //.Select(p => p) //是否可以通过判断类型来取属性，例如只取简单类型的属性
                    .ToArray();
            }
        }

        public static object[] getListByResource(string ResourceID)
        {
            using (MyDB mydb = new MyDB())
            {
                return mydb.Privileges.Where(p=>p.resource.ID.Equals(ResourceID))
                    .Select(p => new { p.ID, p.resource.resourceName, p.resource.module.moduleName, p.privilegeName, p.privilegeCode, p.needAuth, p.isMenuEntry, p.createdTime, p.orderNO }) //快速匿名对象
                    //.Select(p => p) //是否可以通过判断类型来取属性，例如只取简单类型的属性
                    .ToArray();
            }
        }

        public static object[] getModuleResourceTreeData()
        {
            using (MyDB mydb = new MyDB())
            {
                return mydb.Modules.OrderBy(m=>m.OrderNO).Select(
                        m => new
                        {
                            id = m.ID,
                            text = m.moduleName + "[" + m.moduleCode + "]",
                            type = "module",
                            expanded = true,
                            children = m.resources.OrderBy(r=>r.orderNO).Select(
                                r => new
                                {
                                    id = r.ID,
                                    text = r.resourceName + "[" + r.resourceCode + "]",
                                    type = "resource",
                                    leaf = true
                                }
                            )
                        }
                    ).ToArray();
            }
        }

        public static object[] getRoles()
        {
            using (MyDB mydb = new MyDB())
            {
                return mydb.Roles.Select(
                    r => new
                    {
                        r.ID,
                        r.Name,
                        privileges = r.RolePrivileges.Select(p => new { p.Privilege.ID, p.Privilege.privilegeCode, p.Privilege.privilegeName }).ToArray()
                    }).ToArray();
            }
        }
    }
}
