using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EntityObjectContext;
using EntityObjectLib;

namespace BuizApp.Models
{
    public class LoginModel
    {
        string _name;
        [DisplayName("用户名")]
        [Required(ErrorMessage = "必须输入用户名!")]
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _pwd;
        [DisplayName("口令")]
        public string pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        /// <summary>
        /// 登录认证
        /// </summary>
        /// <returns>认证成功,返回用户ID;失败,返回空值</returns>
        public string Auth()
        {
            using (MyDB mydb = new MyDB())
            {
                IQueryable<User> users = mydb.Users.Where(
                    p => p.Code.ToLower().Equals(this.name.ToLower()) && p.Password.ToLower().Equals(this.pwd.ToLower()));
                if (users.Count() == 1)
                {
                    return users.First().ID;
                }
            }
            return string.Empty;
        }

        //public static Dictionary<KeyValuePair<string, string>, List<KeyValuePair<string, string>>> getMenuItems(string UserID)
        //{
        //    using (MyDB mydb = new MyDB())
        //    {
        //        // linq to EF方式应该怎么写？
        //        IQueryable<Privilege> MenuItems = mydb.Privileges.Where(p =>
        //            p.Roles.Where(r => r.Users.Where(u => u.ID.Equals(UserID)).Count() > 0).Count() > 0
        //            );

        //        Dictionary<KeyValuePair<string, string>, List<KeyValuePair<string, string>>> menus = new Dictionary<KeyValuePair<string, string>, List<KeyValuePair<string, string>>>();
        //        foreach (Privilege p in MenuItems)
        //        {
        //            KeyValuePair<string, string> key = new KeyValuePair<string, string>(p.resourceCode, p.resourceName);
        //            if (menus.ContainsKey(key))
        //            {
        //                menus[key].Add(new KeyValuePair<string, string>(p.privilegeCode, p.privilegeName));
        //            }
        //            else
        //            {
        //                menus.Add(key, new[] { new KeyValuePair<string, string>(p.privilegeCode, p.privilegeName) }.ToList());
        //            }
        //        }
        //        return menus;
        //    }
        //}

        /// <summary>
        /// 获得指定用户的菜单数据
        /// </summary>
        /// <param name="userId">指定用户ID</param>
        /// <returns>
        /// 用户的菜单列表
        /// key:资源名称
        /// value:操作名称，操作编码，资源编码，模块编码｝，用于生成操作的链接
        /// </returns>
        public static Dictionary<string, List<string[]>> getMenusData(string userId)
        {
            using (MyDB mydb = new MyDB())
            {
                return
                    mydb.Users.FirstOrDefault(u => u.ID.Equals(userId))
                    .Roles.SelectMany(r => r.Privileges).Where(p => p.isMenuEntry)
                    .GroupBy(p => p.resource)
                    .OrderBy(r => r.Key.orderNO)
                    .ToDictionary(
                        p => p.Key.resourceName
                        , v => v.OrderBy(p => p.orderNO).Select(p => new string[] { 
                            p.privilegeName, 
                            p.privilegeCode, 
                            p.resource.resourceCode, 
                            p.resource.module != null ? p.resource.module.moduleCode : string.Empty 
                        }).ToList()
                        );
            }
        }

        /// <summary>
        /// 判断指定用户是否对某个操作具有权限
        /// </summary>
        /// <param name="userId">指定用户ID</param>
        /// <param name="areaName">区域（模块）名称</param>
        /// <param name="controllerName">控制器（资源）名称</param>
        /// <param name="actionName">行为（操作）名称</param>
        /// <returns></returns>
        public static bool UserHasPrivilege(string userId, string areaName, string controllerName, string actionName)
        {
            /*
             * 判断
             * 1.权限是否需要认证，如果不需要，通过
             * 2.如果需要认证，看用户的角色权限里是否包含该行为，如果包含，通过，不包含，否决
             */
            bool isAuthorizaion = false;
            using (MyDB mydb = new MyDB())
            {
                //在sqlite中，忽略大小写设置无效????
                Privilege privilege = mydb.Privileges.Where(
                    p => (string.IsNullOrEmpty(areaName) ? p.resource.module == null : p.resource.module.moduleCode.ToLower().Equals(areaName.ToLower()))
                        && p.resource.resourceCode.ToLower().Equals(controllerName.ToLower())
                        && p.privilegeCode.ToLower().Equals(actionName.ToLower())
                        ).FirstOrDefault();
                if (privilege != null)
                {
                    if (!privilege.needAuth)
                    {
                        isAuthorizaion = true;
                    }
                    else
                    {
                        isAuthorizaion = privilege.Roles
                            .Where(r => r.Users.Where(u => u.ID.Equals(userId)).Count() > 0)
                            .Count() > 0;
                    }
                }
            }
            return isAuthorizaion;
        }

        /// <summary>
        /// 获得指定用户的名称
        /// </summary>
        /// <param name="userId">指定用户ID</param>
        /// <returns>用户名称</returns>
        public static string getUserName(string userId)
        {
            using (MyDB mydb = new MyDB())
            {
                return mydb.Users.FirstOrDefault(u => u.ID.Equals(userId)).Name;
            }
        }
    }
}