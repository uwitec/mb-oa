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
using System.Data.Entity;

namespace TestProject1
{
    [TestClass]
    public class MenuInit
    {
        public void TestTree()
        {
            MyDB mydb = new MyDB();

            OrganizationExt m = mydb.OrganizationExts.FirstOrDefault();
            Debug.WriteLine(m.Name);
        }

        [TestMethod]
        public void ClearData()
        {
            using (MyDB mydb = new MyDB())
            {
                IQueryable<Role> roles = mydb.Roles.Select(p => p);
                foreach (Role r in roles)
                {
                    r.RolePrivileges.Clear();
                    r.Subjects.OfType<User>().ToList().Clear();
                    mydb.Roles.Remove(r);
                }

                //IQueryable<Privilege> privileges = mydb.Privileges.Select(p => p);
                //foreach (Privilege r in privileges)
                //{
                //    mydb.Privileges.Remove(r);
                //}

                mydb.Privileges.Load();
                mydb.Privileges.Local.Clear();

                //IQueryable<Resource> resources = mydb.Resources.Select(p => p);
                //foreach (Resource r in resources)
                //{
                //    mydb.Resources.Remove(r);
                //}

                mydb.Resources.Load();
                mydb.Resources.Local.Clear();

                //IQueryable<Module> modules = mydb.Modules.Select(p => p);
                //foreach (Module m in modules)
                //{
                //    mydb.Modules.Remove(m);
                //}

                mydb.Modules.Load();
                mydb.Modules.Local.Clear();

                //IQueryable<User> users = mydb.Users.Select(p => p);
                //foreach (User r in users)
                //{
                //    mydb.Users.Remove(r);
                //}

                mydb.Users.Load();
                mydb.Users.Local.Clear();

                //foreach (Organization o in mydb.Organizations.Select(o => o).OrderByDescending(o=>o.OrderNO))
                //{
                //    mydb.Organizations.Remove(o);
                //}

                mydb.Organizations.Load();
                foreach (Organization org in mydb.Organizations.Local)
                {
                    if (org.Users != null)
                    {
                        org.Users.Clear();
                    }
                }
                mydb.Organizations.Local.Clear();

                mydb.UserGroups.Load();
                foreach (UserGroup ug in mydb.UserGroups.Local)
                {
                    if (ug.Users != null)
                    {
                        ug.Users.Clear();
                    }
                }
                mydb.UserGroups.Local.Clear();

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
                                        privilegeName = "组织机构管理",
                                        privilegeCode = "organization",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 30
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
                                        orderNO = 40
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
                                        orderNO = 50
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
                                        orderNO = 100
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
                        ID = Guid.NewGuid().ToString(), moduleCode = "Office", moduleName = "日常办公",
                        resources = new Resource[]{
                            new Resource{ ID = Guid.NewGuid().ToString(),resourceCode = "myOffice",resourceName = "个人办公", orderNO=100,
                                privileges = new Privilege[] {
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "我的消息",
                                        privilegeCode = "myMessage",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 1
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "公告板",
                                        privilegeCode = "boards",
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
                            new Resource{ ID = Guid.NewGuid().ToString(),resourceCode = "orgOffice",resourceName = "行政办公", orderNO=150,
                                privileges = new Privilege[] {
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "文件柜",
                                        privilegeCode = "orgDocument",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 10
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "通知公告",
                                        privilegeCode = "orgNotice",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 20
                                    },
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "讨论板",
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
                            new Resource{ ID = Guid.NewGuid().ToString(),resourceCode = "equipment",resourceName = "资源管理",orderNO =160,
                                privileges = new Privilege[] {
                                    new Privilege {
                                        ID = Guid.NewGuid().ToString(),
                                        privilegeName = "资源台帐",
                                        privilegeCode = "account",
                                        needAuth = true,
                                        isMenuEntry = true,
                                        createdTime = DateTime.Now,
                                        securityGrade = 1,
                                        orderNO = 1
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
                                        privilegeName = "资源占用情况",
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
                                        privilegeCode = "define",
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
                    roleCode = "admin" ,
                    roleName = "管理员"
                },
                new Role { 
                    ID = Guid.NewGuid().ToString(), 
                    roleCode = "normal user",
                    roleName = "一般用户"
                },
                new Role { 
                    ID = Guid.NewGuid().ToString(), 
                    roleCode = "guest" ,
                    roleName = "访问者"
                }
            };

            Role admin = roles.FirstOrDefault(r => r.roleCode.Equals("admin"));
            IEnumerable<Privilege> allPprivilieges = modules.SelectMany(p => p.resources).SelectMany(r=>r.privileges);
            admin.RolePrivileges = new List<RolePrivilege>();
            foreach (Privilege p in allPprivilieges)
            {
                admin.RolePrivileges.Add(
                    new RolePrivilege
                    {
                        ID = Guid.NewGuid().ToString(),
                        Privilege = p,
                        Parameters = ""
                    }
                    );
            }
            Role normal_user = roles.FirstOrDefault(r => r.roleCode.Equals("normal user"));
            IEnumerable<Privilege> defaultPprivilieges = modules.Where(m => m.moduleCode == "report").SelectMany(p => p.resources).SelectMany(r => r.privileges);
            normal_user.RolePrivileges = new List<RolePrivilege>();
            foreach (Privilege p in defaultPprivilieges)
            {
                normal_user.RolePrivileges.Add(
                    new RolePrivilege
                    {
                        ID = Guid.NewGuid().ToString(),
                        Privilege = p,
                        Parameters = "normal user"
                    }
                    );
            }

            Role guest = roles.FirstOrDefault(r => r.roleCode.Equals("guest"));
            IEnumerable<Privilege> guestPrivilieges = modules.Where(m => m.moduleCode == "Office" || m.moduleCode == "default").SelectMany(p => p.resources).SelectMany(r => r.privileges);
            guest.RolePrivileges = new List<RolePrivilege>();
            foreach (Privilege p in guestPrivilieges)
            {
                normal_user.RolePrivileges.Add(
                    new RolePrivilege
                    {
                        ID = Guid.NewGuid().ToString(),
                        Privilege = p,
                        Parameters = "guest"
                    }
                    );
            }

            Organization[] orgs = new Organization[]{
                  new Organization{ 
                      ID = Guid.NewGuid().ToString(), 
                      Code = "futuresoft",
                      Name = "某某公司",
                      OrderNO = 10,
                      Roles= roles.Where(r=>r.roleCode=="guest").ToArray(),
                      Children = new Organization[]{
                          new Organization{
                              ID = Guid.NewGuid().ToString(), 
                              Code = "dev",
                              Name = "开发部",
                              OrderNO = 10,
                              Children = new Organization[]{
                                  new Organization{
                                      ID = Guid.NewGuid().ToString(), 
                                      Code = "JJ",
                                      Name = "计价软件项目组",
                                      OrderNO = 100
                                  },
                                  new Organization{
                                      ID = Guid.NewGuid().ToString(), 
                                      Code = "GJ",
                                      Name = "工程量与钢筋项目组",
                                      OrderNO = 110
                                  },
                                  new Organization{
                                      ID = Guid.NewGuid().ToString(), 
                                      Code = "MIS",
                                      Name = "MIS项目组",
                                      OrderNO = 120,
                                      Users = new User[]{
                                          new User{
                                                ID=Guid.NewGuid().ToString(), 
                                                Code="lilin", 
                                                Name="李林", 
                                                Password="123456",
                                                Roles= roles.Where(r=>r.roleCode=="admin").ToArray()
                                            }
                                      }
                                  }
                              },
                              Roles= roles.Where(r=>r.roleCode=="normal user").ToArray(),
                              Users = new User[]{
                                    new User{
                                        ID=Guid.NewGuid().ToString(), 
                                        Code="chw", 
                                        Name="陈宏伟", 
                                        Password="123456"
                                    }
                              }
                          },
                          new Organization{
                              ID = Guid.NewGuid().ToString(), 
                              Code = "Market",
                              Name = "市场部",
                              OrderNO = 20,
                              Children = new Organization[]{
                                  new Organization{
                                      ID = Guid.NewGuid().ToString(), 
                                      Code = "NJ",
                                      Name = "南京组",
                                      OrderNO = 200
                                  },
                                  new Organization{
                                      ID = Guid.NewGuid().ToString(), 
                                      Code = "SZ",
                                      Name = "苏州组",
                                      OrderNO = 210
                                  },
                                  new Organization{
                                      ID = Guid.NewGuid().ToString(), 
                                      Code = "FZ",
                                      Name = "非洲组",
                                      OrderNO = 220
                                  }
                              }
                          },
                          new Organization{
                              ID = Guid.NewGuid().ToString(), 
                              Code = "Test",
                              Name = "测试部",
                              OrderNO = 30
                          },
                          new Organization{
                              ID = Guid.NewGuid().ToString(), 
                              Code = "apply",
                              Name = "实施部",
                              OrderNO = 50,
                              Children = new Organization[]{
                                  new OrganizationExt{
                                      ID = Guid.NewGuid().ToString(), 
                                      Code = "One",
                                      Name = "实施一组",
                                      ExtName = "一组附加信息",
                                      OrderNO = 500
                                  },
                                  new Organization{
                                      ID = Guid.NewGuid().ToString(), 
                                      Code = "two",
                                      Name = "实施二组",
                                      OrderNO = 510
                                  },
                                  new Organization{
                                      ID = Guid.NewGuid().ToString(), 
                                      Code = "three",
                                      Name = "实施三组",
                                      OrderNO = 520
                                  }
                          },
                      }
                  }
                  }
            };

            using (MyDB mydb = new MyDB())
            {
                // 奇怪,下面的foreach不能少,不然会出现"操作到资源"关系缺失的错误提示
                foreach (Module m in modules)
                {
                    mydb.Modules.Add(m);
                }

                foreach (Organization org in orgs)
                {
                    mydb.Organizations.Add(org);
                }

                UserGroup[] ugs = new UserGroup[]{
                    new UserGroup
                    {
                        ID = Guid.NewGuid().ToString(),
                        Code = "UG-A",
                        Name = "A用户组",
                        Description = "A用户组描述",
                        //QQ="123123123",
                        Users = mydb.Users.Local.ToArray()
                    },
                    new UserGroup
                    {
                        ID = Guid.NewGuid().ToString(),
                        Code = "UG-B",
                        Name = "B用户组",
                        Description = "B用户组描述",
                        //QQ="23423423",
                        Users = mydb.Users.Local.Where(u=>u.Code.Equals("chw")).ToArray()
                    },
                };

                foreach (UserGroup ug in ugs)
                {
                    mydb.UserGroups.Add(ug);
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

        [TestMethod]
        public void testSubject()
        {
            using (MyDB mydb = new MyDB())
            {
                ICollection<User> us = mydb.Roles.First().Subjects.OfType<User>().ToList() ;
                IQueryable<User> us1 = mydb.Roles.SelectMany(r => r.Subjects).OfType<User>();
                IQueryable<User> users = mydb.Roles.SelectMany(r => r.Subjects.OfType<User>());
                //IQueryable < User > users = mydb.Roles.SelectMany(r => r.Subjects).Distinct().Where(s => s.Category.Equals("User")).Select(s => s as User);
                //IQueryable<Organization> Organizations = mydb.Roles.SelectMany(r => r.Organizations);
                return;
            }
        }
    }
}
