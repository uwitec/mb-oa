using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BuizApp.Models;
using System.Web.Security;

namespace BuizApp
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        //[HttpGet]
        public ActionResult Index()
        {
            LoginModel lm = new LoginModel();
            lm.name = "lilin";
            lm.pwd = "123456";
            lm.other = "other";
            return View(lm);
        }

        [HttpPost]
        public ActionResult Index(LoginModel lm)
        {
            /* 此外的lm不是在HttpGet时传入的lm，是新建的LoginModel对象，
             * 根据form下的同名属性进行了赋值，
             * 这个lm没有以任何形式在会话之间保持
             * 相当于：
            LoginModel lm = new LoginModel();
            lm.name = Request.Form["name"];
            lm.pwd = Request.Form["pwd"];*/

            string UserID = lm.Auth();

            if (!string.IsNullOrEmpty(UserID))
            {
                // 验证成功，应该转到LoginInSuccess，并带上UserID
                TempData["UserID"] = UserID; //TempData可以在跳转时保持，而ViewData不能

                //方法一
                //FormsAuthentication.RedirectFromLoginPage(UserID, false);

                //方法二
                FormsAuthentication.SetAuthCookie(UserID, false);

                //方法一，推荐
                //return View(lm);
                return RedirectToAction("Index", "Main"); //转到应用主界面

                //方法二
                //RedirectToRouteResult r = RedirectToAction("LoginInSuccess");
                //r.ExecuteResult(this.ControllerContext);
                //return null;



            }
            else
            {
                // 验证失败，返回登录页
                //方法一，可转向到某action
                return RedirectToAction("index");

                //方法二，可转向任何网址
                //return new RedirectResult("/Login");

                //返回登录失败提示（字符串）
                //ContentResult cr = new ContentResult();
                //cr.Content="验证失败!";
                //return cr;
            }
        }
    }
}
