using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using EntityObjectLib;
using BuizApp.App_Code;

namespace BuizApp.Areas.msgService.Controllers
{
    public class sendMessageController : Controller
    {
        //
        // GET: http://localhost:12480/msgService/sendMessage
        /// <summary>
        /// 发消息,只接收POST
        /// sendMessage(userCode,passwd, title, content, to) //发消息给指定用户,to是用户ID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Index()
        {
            string userCode = Request.Form["user"];
            string password = Request.Form["pwd"];
            string title = Request.Form["title"];
            string content = Request.Form["content"];
            string to = Request.Form["to"];

            Response.Clear();
            Response.ContentType = "text/xml";

            string infoID = Guid.NewGuid().ToString();
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.User user = mydb.Users.First(u => u.Code.ToLower() == userCode.ToLower() && u.Password == password);
                if (user != null)
                {
                    EntityObjectLib.Info info = new Info
                    {
                        ID = infoID,
                        Title = title,
                        Content = content,
                        CreateDate = DateTime.Now,
                        SendDate = DateTime.Now,
                        Creator = user,
                        SendTypes = "",
                        Receivers = new EntityObjectLib.InfoInbox[]{ 
                        new EntityObjectLib.InfoInbox
                        {
                            ID = Guid.NewGuid().ToString(),
                            Receiver = mydb.Users.Find(to),
                            ReceiveTypes = ""
                        }}
                    };

                    mydb.Infos.Add(info);
                    mydb.SaveChanges();

                    return CJX.Object2XML(new { result = true }).OuterXml;
                }
                else
                {
                    return "<?xml version=\"1.0\"?><root><error>用户名口令错误!</error></root>";
                }
            }
        }
    }
}
