using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using EntityObjectLib;
using EntityObjectContext;
using System.Data.Entity.Validation;
using System.Data;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {

        public void TestMethod1()
        {
            MyDB mydb = new MyDB();

            Movie m = mydb.Movies.FirstOrDefault();
            Debug.WriteLine(m.Title);
        }


        public void Insert()
        {
            using (MyDB mydb = new MyDB())
            {
                Category c = new Category { Code = "asd", Name = "asdasd" };
                mydb.Categories.Add(c);

                //mydb.Categories.Add(new Category { Code="asd", Name="asdasd" });

                int r = mydb.SaveChanges();

                Debug.WriteLine(r);
            }
        }


        public void TestTree()
        {
            MyDB mydb = new MyDB();

            CategoryExt m = mydb.CategoriesExt.FirstOrDefault();
            Debug.WriteLine(m.Name);
        }

        [TestMethod]
        [TestCategory("Data append")]
        public void AppendData()
        {
            ClearData();

            using (MyDB mydb = new MyDB())
            {
                #region 模块{m1:系统管理,m2：销售业务管理}
                Module m1 = new Module
                {
                    ID = Guid.NewGuid().ToString(),
                    moduleCode = "system",
                    moduleName = "系统管理"
                };
                mydb.Modules.Add(m1);
                Module m2 = new Module
                {
                    ID = Guid.NewGuid().ToString(),
                    moduleCode = "test",
                    moduleName = "销售业务管理"
                };
                mydb.Modules.Add(m2);
                Module m3 = new Module
                {
                    ID = Guid.NewGuid().ToString(),
                    moduleCode = "default",
                    moduleName = "缺省"
                };
                mydb.Modules.Add(m3);
                Module m4 = new Module
                {
                    ID = Guid.NewGuid().ToString(),
                    moduleCode = "data",
                    moduleName = "数据服务"
                };
                mydb.Modules.Add(m4);
                #endregion

                #region 资源{res1:系统管理.权限管理,res2:系统管理.日志管理,res3:销售业务管理.订单管理,res4销售业务管理.库存管理}
                Resource res1 = new Resource
                {
                    ID = Guid.NewGuid().ToString(),
                    resourceCode = "auth",
                    resourceName = "权限管理"
                };
                mydb.Resources.Add(res1);
                Resource res2 = new Resource
                {
                    ID = Guid.NewGuid().ToString(),
                    resourceCode = "log",
                    resourceName = "日志查询"
                };
                mydb.Resources.Add(res2);


                Resource res3 = new Resource
                {
                    ID = Guid.NewGuid().ToString(),
                    resourceCode = "order",
                    resourceName = "订单管理"
                };
                mydb.Resources.Add(res3);
                Resource res4 = new Resource
                {
                    ID = Guid.NewGuid().ToString(),
                    resourceCode = "kuCun",
                    resourceName = "库存管理"
                };
                mydb.Resources.Add(res4);
                Resource res5 = new Resource
                {
                    ID = Guid.NewGuid().ToString(),
                    resourceCode = "Login",
                    resourceName = "登录"
                };
                mydb.Resources.Add(res5);
                Resource res6 = new Resource
                {
                    ID = Guid.NewGuid().ToString(),
                    resourceCode = "Main",
                    resourceName = "主界面"
                };
                mydb.Resources.Add(res6);
                Resource res7 = new Resource
                {
                    ID = Guid.NewGuid().ToString(),
                    resourceCode = "Privilege",
                    resourceName = "权限"
                };
                mydb.Resources.Add(res7);
                #endregion

                #region 权限 系统管理.权限管理
                Privilege p1 = new Privilege
                {
                    ID = Guid.NewGuid().ToString(),
                    privilegeName = "权限",
                    privilegeCode = "privilege",
                    needAuth = true,
                    isMenuEntry = true,
                    createdTime = DateTime.Now,
                    securityGrade = 1,
                    orderNO = 1
                };
                mydb.Privileges.Add(p1);
                Privilege p2 = new Privilege
                {
                    ID = Guid.NewGuid().ToString(),
                    privilegeName = "角色",
                    privilegeCode = "role",
                    needAuth = true,
                    isMenuEntry = true,
                    createdTime = DateTime.Now,
                    securityGrade = 1,
                    orderNO = 2
                };
                mydb.Privileges.Add(p2);

                Privilege p3 = new Privilege
                {
                    ID = Guid.NewGuid().ToString(),
                    privilegeName = "创建新订单",
                    privilegeCode = "create",
                    needAuth = true,
                    isMenuEntry = true,
                    createdTime = DateTime.Now,
                    securityGrade = 1,
                    orderNO = 100
                };
                mydb.Privileges.Add(p3);
                Privilege p4 = new Privilege
                {
                    ID = Guid.NewGuid().ToString(),
                    privilegeName = "审核订单",
                    privilegeCode = "audit",
                    needAuth = true,
                    isMenuEntry = true,
                    createdTime = DateTime.Now,
                    securityGrade = 1,
                    orderNO = 101
                };
                mydb.Privileges.Add(p4);

                Privilege p5 = new Privilege
                {
                    ID = Guid.NewGuid().ToString(),
                    privilegeName = "操作日志查询",
                    privilegeCode = "query",
                    needAuth = true,
                    isMenuEntry = true,
                    createdTime = DateTime.Now,
                    securityGrade = 1,
                    orderNO = 120
                };
                mydb.Privileges.Add(p5);

                Privilege p6 = new Privilege
                {
                    ID = Guid.NewGuid().ToString(),
                    privilegeName = "登录",
                    privilegeCode = "Index",
                    needAuth = false,
                    isMenuEntry = false,
                    createdTime = DateTime.Now,
                    securityGrade = 1,
                    orderNO = 120
                };
                mydb.Privileges.Add(p6);

                Privilege p7 = new Privilege
                {
                    ID = Guid.NewGuid().ToString(),
                    privilegeName = "主界面",
                    privilegeCode = "Index",
                    needAuth = true,
                    isMenuEntry = false,
                    createdTime = DateTime.Now,
                    securityGrade = 1,
                    orderNO = 120
                };
                mydb.Privileges.Add(p7);
                Privilege p8 = new Privilege
                {
                    ID = Guid.NewGuid().ToString(),
                    privilegeName = "权限表",
                    privilegeCode = "Index",
                    needAuth = false,
                    isMenuEntry = false,
                    createdTime = DateTime.Now,
                    securityGrade = 1,
                    orderNO = 120
                };
                mydb.Privileges.Add(p8);
                #endregion

                #region 用户角色
                Role R1 = new Role { ID = Guid.NewGuid().ToString(), Name = "Role1" };
                mydb.Roles.Add(R1);
                Role R2 = new Role { ID = Guid.NewGuid().ToString(), Name = "Role2" };
                mydb.Roles.Add(R2);
                Role R3 = new Role { ID = Guid.NewGuid().ToString(), Name = "Role3" };
                mydb.Roles.Add(R3);

                User u1 = new User { ID = Guid.NewGuid().ToString(), Code = "chenhongwei", Name = "陈宏伟", Password = "123456" };
                mydb.Users.Add(u1);
                User u2 = new User { ID = Guid.NewGuid().ToString(), Code = "lilin", Name = "李琳", Password = "123456" };
                mydb.Users.Add(u2);
                #endregion
                m1.resources = new[] { res1, res2 };
                m2.resources = new[] { res3, res4 };
                m3.resources = new[] { res5, res6 };
                m4.resources = new[] { res7 }; ;
                res1.privileges = new[] { p1, p2 };
                res3.privileges = new[] { p3, p4 };
                res2.privileges = new[] { p5 };
                res5.privileges = new[] { p6 };
                res6.privileges = new[] { p7 };
                res7.privileges = new[] { p8 };
                R1.Privileges = new Privilege[] { p1, p2, p7 };
                R2.Privileges = new Privilege[] { p1, p3, p4, p7 };
                R3.Privileges = new Privilege[] { p5, p7 };
                u1.Roles = new Role[] { R1, R2 };
                u2.Roles = new Role[] { R2, R3 };

                try
                {
                    int r = mydb.SaveChanges();
                    Debug.WriteLine(r);
                }
                catch (DbEntityValidationException ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
        }

        [TestMethod]
        public void ClearData()
        {
            using (MyDB mydb = new MyDB())
            {
                IQueryable<Role> roles = mydb.Roles.Select(p => p);
                foreach (Role r in roles)
                {
                    r.Privileges.Clear();
                    r.Users.Clear();
                    mydb.Roles.Remove(r);
                }

                IQueryable<Privilege> privileges = mydb.Privileges.Select(p => p);
                foreach (Privilege r in privileges)
                {
                    mydb.Privileges.Remove(r);
                }

                IQueryable<Resource> resources = mydb.Resources.Select(p => p);
                foreach (Resource r in resources)
                {
                    mydb.Resources.Remove(r);
                }

                IQueryable<Module> modules = mydb.Modules.Select(p => p);
                foreach (Module m in modules)
                {
                    mydb.Modules.Remove(m);
                }

                IQueryable<User> users = mydb.Users.Select(p => p);
                foreach (User r in users)
                {
                    mydb.Users.Remove(r);
                }

                int result = mydb.SaveChanges();

                Debug.WriteLine(result);
            }
        }

        [TestMethod]
        public void AppendData1()
        {
            this.ClearData();
            Module[] modules = new Module[]
                {
#region 系统管理
                    new Module 
                    {
                        ID = Guid.NewGuid().ToString(),                    
                        moduleCode = "system",                    
                        moduleName = "系统管理",
                        resources = new Resource[]
                        {
                            new Resource
                            {
                                ID = Guid.NewGuid().ToString(),
                                resourceCode = "auth",
                                resourceName = "权限管理",
                                orderNO = 1000,
                                privileges = new Privilege[]
                                {
                                    new Privilege
                                    {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "模块功能操作管理",
                                        privilegeCode = "privilege",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 10
                                    },
                                    new Privilege
                                    {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "角色管理",
                                        privilegeCode = "role",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 20
                                    },
                                    new Privilege
                                    {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "用户管理",
                                        privilegeCode = "user",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 30
                                    },
                                    new Privilege
                                    {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "分级授权",
                                        privilegeCode = "selfAuth",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 40
                                    },
                                    new Privilege
                                    {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "权限查询",
                                        privilegeCode = "query",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 50
                                    }
                                }
                            },
                            new Resource
                            {
                                ID = Guid.NewGuid().ToString(),
                                resourceCode = "log",
                                resourceName = "日志查询",
                                orderNO = 900,
                                privileges = new Privilege[]
                                {
                                    new Privilege
                                    {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "操作日志查询",
                                        privilegeCode = "query",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 120
                                    }
                                }
                            }
                        }
                    },
#endregion
                    new Module 
                    {
                        ID = Guid.NewGuid().ToString(),                    
                        moduleCode = "default",                    
                        moduleName = "缺省",
                        resources = new Resource[]
                        {
                            new Resource
                            {
                                ID = Guid.NewGuid().ToString(),
                                resourceCode = "Login",
                                resourceName = "登录",
                                orderNO = 0,
                                privileges = new Privilege[]
                                {
                                    new Privilege
                                    {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "登录",
                                        privilegeCode = "Index",
                                        needAuth = false,
                                        isMenuEntry = false,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 120
                                    }
                                }
                            },
                            new Resource
                            {
                                ID = Guid.NewGuid().ToString(),
                                resourceCode = "Main",
                                resourceName = "主界面",
                                orderNO = 10,
                                privileges = new Privilege[]
                                {
                                    new Privilege
                                    {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "主界面",
                                        privilegeCode = "Index",
                                        needAuth = true,
                                        isMenuEntry = false,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 120
                                    }
                                }
                            }
                        }
                    },
                    new Module 
                    {
                        ID = Guid.NewGuid().ToString(),                    
                        moduleCode = "data",                    
                        moduleName = "数据服务",
                        resources = new Resource[]
                        {
                            new Resource
                            {
                                ID = Guid.NewGuid().ToString(),
                                resourceCode = "Privilege",
                                resourceName = "权限",
                                orderNO = 30,
                                privileges = new Privilege[]
                                {
                                    new Privilege
                                    {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "权限表",
                                        privilegeCode = "Index",
                                        needAuth = false,
                                        isMenuEntry = false,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 120
                                    },
                                    new Privilege
                                    {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "角色表",
                                        privilegeCode = "role",
                                        needAuth = false,
                                        isMenuEntry = false,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 120
                                    },
                                    new Privilege
                                    {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "模块功能树",
                                        privilegeCode = "moduleResourceTree",
                                        needAuth = false,
                                        isMenuEntry = false,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 120
                                    }
                                }
                            }
                        }
                    },
                    new Module{
                        ID = Guid.NewGuid().ToString(), moduleCode = "message", moduleName = "消息管理",
                        resources = new Resource[]{
                            new Resource{ ID = Guid.NewGuid().ToString(),resourceCode = "notice",resourceName = "通知公告",orderNO = 200,
                                privileges = new Privilege[] {
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "通知公告",
                                        privilegeCode = "Index",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 120
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "公告管理[本部门]",
                                        privilegeCode = "create",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 120
                                    }
                                }
                            }
                        }
                    },
                    new Module{
                        ID = Guid.NewGuid().ToString(), moduleCode = "Office", moduleName = "日常办公",
                        resources = new Resource[]{
                            new Resource{ ID = Guid.NewGuid().ToString(),resourceCode = "myOffice",resourceName = "个人办公", orderNO=100,
                                privileges = new Privilege[] {
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "我的文件柜",
                                        privilegeCode = "myDocument",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 10
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "我的日程",
                                        privilegeCode = "myCarlendar",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 20
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "我的通讯录",
                                        privilegeCode = "myAddressBook",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 30
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "我的便笺",
                                        privilegeCode = "myMemo",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 40
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "我的任务",
                                        privilegeCode = "myTask",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 50
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "我的提醒",
                                        privilegeCode = "myRemind",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 60
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "申请授权",
                                        privilegeCode = "myApplyPrivilege",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 70
                                    }
                                }
                            },
                            new Resource{ ID = Guid.NewGuid().ToString(),resourceCode = "orgOffice",resourceName = "部门办公", orderNO=150,
                                privileges = new Privilege[] {
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "部门文件柜",
                                        privilegeCode = "orgDocument",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 10
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "部门公告板",
                                        privilegeCode = "orgNotice",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 20
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "工作计划",
                                        privilegeCode = "orgPlan",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 30
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "工作小结",
                                        privilegeCode = "orgSummary",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 40
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "预警",
                                        privilegeCode = "orgAlarm",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 50
                                    }
                                }
                            }
                        }
                    },
                    new Module{
                        ID = Guid.NewGuid().ToString(), moduleCode = "resource", moduleName = "资源管理",
                        resources = new Resource[]{
                            new Resource{ ID = Guid.NewGuid().ToString(),resourceCode = "equipment",resourceName = "设备管理",orderNO =160,
                                privileges = new Privilege[] {
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "预约",
                                        privilegeCode = "subscribe",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 10
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "申请",
                                        privilegeCode = "apply",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 20
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "审核",
                                        privilegeCode = "audit",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 30
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "分配",
                                        privilegeCode = "assign",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 40
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "领用记录",
                                        privilegeCode = "get",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 50
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "回收",
                                        privilegeCode = "retrieve",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 50
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "人工调度",
                                        privilegeCode = "schedule",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 50
                                    }
                                }
                            }
                        }
                    },
                    new Module{
                        ID = Guid.NewGuid().ToString(), moduleCode = "report", moduleName = "查询报表",
                        resources = new Resource[]{
                            new Resource{ ID = Guid.NewGuid().ToString(),resourceCode = "report",resourceName = "查询报表",orderNO=600,
                                privileges = new Privilege[] {
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "设备占用情况",
                                        privilegeCode = "query1",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 10
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "query2",
                                        privilegeCode = "apply",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 20
                                    }
                                }
                            },
                            new Resource{ ID = Guid.NewGuid().ToString(),resourceCode = "custom",resourceName = "自定义查询报表",orderNO = 500,
                                privileges = new Privilege[] {
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "报表数据源管理",
                                        privilegeCode = "dataSource",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 10
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "自定义报表",
                                        privilegeCode = "define",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 20
                                    }
                                }
                            }
                        }
                    },
                    new Module{
                        ID = Guid.NewGuid().ToString(), moduleCode = "workflow", moduleName = "流程",
                        resources = new Resource[]{
                            new Resource{ ID = Guid.NewGuid().ToString(),resourceCode = "workflowManage",resourceName = "流程管理",orderNO = 300,
                                privileges = new Privilege[] {
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "流程定义",
                                        privilegeCode = "query1",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 10
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "流程监控",
                                        privilegeCode = "apply",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 20
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "自定义表单",
                                        privilegeCode = "apply",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 20
                                    }
                                }
                            },
                            new Resource{ ID = Guid.NewGuid().ToString(),resourceCode = "myWorkflow",resourceName = "我的流程",orderNO = 400,
                                privileges = new Privilege[] {
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "发起流程",
                                        privilegeCode = "dataSource",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 10
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "待办事项",
                                        privilegeCode = "define",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 20
                                    }
                                }
                            }
                        }
                    }
                };

            Role[] roles = new Role[]{
                new Role { 
                    ID = Guid.NewGuid().ToString(), 
                    Name = "admin" ,
                    Privileges = modules.SelectMany(p=>p.resources).SelectMany(r=>r.privileges).ToArray()
                },
                new Role { 
                    ID = Guid.NewGuid().ToString(), 
                    Name = "normal user",
                    Privileges = modules.Where(m=>m.moduleCode=="default").SelectMany(p=>p.resources).SelectMany(r=>r.privileges).ToArray()
                },
                new Role { 
                    ID = Guid.NewGuid().ToString(), 
                    Name = "guest" 
                }
            };

            User[] users = new User[]{
                new User{
                    ID=Guid.NewGuid().ToString(), 
                    Code="lilin", 
                    Name="李林", 
                    Password="123456",
                    Roles= roles.Where(r=>r.Name=="admin").ToArray()
                },
                new User{
                    ID=Guid.NewGuid().ToString(), 
                    Code="chw", 
                    Name="陈宏伟", 
                    Password="123456",
                    Roles= roles.Where(r=>r.Name=="normal user" || r.Name=="guest").ToArray()
                }
            };
            using (MyDB mydb = new MyDB())
            {
                foreach (Module m in modules)
                {
                    mydb.Modules.Add(m);
                }

                foreach (User u in users)
                {
                    mydb.Users.Add(u);
                }
                mydb.SaveChanges();
            }
        }

        [TestMethod]
        public void testLazyLoad()
        {
            using (MyDB mydb = new MyDB())
            {
                IQueryable<Privilege> Privileges = mydb.Privileges.Select(u => u);
                return;
            }
        }
    }
}
