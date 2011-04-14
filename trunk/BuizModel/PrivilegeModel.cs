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
                    .Select(p => new { p.ID, p.resource.resourceName,p.resource.module.moduleName,p.privilegeName, p.privilegeCode, p.orderNO }) //快速匿名对象
                    //.Select(p => p) //是否可以通过判断类型来取属性，例如只取简单类型的属性
                    .ToArray();
            }
        }

        public static object[] getModuleResourceTreeData()
        {
            using (MyDB mydb = new MyDB())
            {
                return mydb.Modules.Select(
                        m => new
                        {
                            id = m.ID,
                            text = m.moduleName + "[" + m.moduleCode + "]",
                            expanded = true,
                            children = m.resources.Select(
                                r => new
                                {
                                    id = r.ID,
                                    text = r.resourceName + "[" + r.resourceCode + "]",
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
