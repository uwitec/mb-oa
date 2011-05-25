using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using BuizApp.App_Code;
using System.Linq.Expressions;
using EntityObjectLib;

namespace BuizApp.Areas.msgService.Controllers
{
    public class getMyMessagesController: Controller
    {
        //
        // GET: http://localhost:12480/msgService/getMyMessages?user=chw&pwd=123456&type=&start=1&length=10
        /// <summary>
        /// 取我的指定类别的全部消息
        /// 其中type,start,length可选,缺省:type为全部类型,start为零,length为100
        /// type:"UR","
        /// </summary>
        /// <returns></returns>
        public string Index()
        {
            List<Expression<Func<EntityObjectLib.Info, bool>>> expressions = new List<Expression<Func<EntityObjectLib.Info, bool>>>();
            string userCode = Request.QueryString["user"];
            string password = Request.QueryString["pwd"];
            string type = Request.QueryString["type"];
            if (!string.IsNullOrEmpty(type))
            {
                expressions.Add(info => info.Type == type);
            }
            int start = Request.QueryString["start"] == null || string.IsNullOrEmpty(Request.QueryString["start"]) ? 1 : int.Parse(Request.QueryString["start"]);
            int length = Request.QueryString["length"] == null || string.IsNullOrEmpty(Request.QueryString["length"]) ? 100 : int.Parse(Request.QueryString["length"]);

            Response.Clear();
            Response.ContentType = "text/xml";

            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.User user = mydb.Users.First(u => u.Code.ToLower() == userCode.ToLower() && u.Password == password);
                if (user!=null)
                {
                    IQueryable<Info> infos = mydb.InfoInboxs
                        .Where(ib => ib.Receiver.ID == user.ID)
                        .Select(ib => ib.Info)
                        .OrderByDescending(info => info.CreateDate);
                        
                        
                    foreach(Expression<Func<EntityObjectLib.Info, bool>> expression in expressions)
                    {
                        infos = infos.Where(expression);
                    }

                    return CJX.Object2XML(
                            infos.Skip(start - 1).Take(length).Select(info => new { info.ID, info.Title, Creator = info.Creator.Name, info.CreateDate, Files = mydb.InfoFiles.Count(f => f.Info.ID == info.ID) })
                            .ToList()
                            .Select(info => new { info.ID, info.Title, info.Creator, CreateDate=info.CreateDate.ToString("yyyy-M-d H:m:s") ,info.Files})
                            .ToArray()
                            ).OuterXml;
                }
                else
                {
                    return "<?xml version=\"1.0\"?><root><error>用户名口令错误!</error></root>";
                }
            }
        }
    }
}
