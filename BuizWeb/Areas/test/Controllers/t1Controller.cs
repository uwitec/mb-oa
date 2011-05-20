using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Configuration;

namespace BuizApp.Areas.test.Controllers
{
    public class t1Controller : Controller
    {
        //
        // GET: /test/t1/

        public ActionResult Index(string Id)
        {
            Response.Write(string.Format("id:{0}<br/>",Id));
            return View();
        }

        public ActionResult test2()
        {
            return View();
        }

        public ActionResult testSwfUpload()
        {
            return View();
        }

        public string upload()
        {
            // 保存传来的文件
            HttpPostedFileBase file = Request.Files["Filedata"]; // 在FileData里
            FileInfo fi = new FileInfo(file.FileName);
            string fileID = Guid.NewGuid().ToString();
            string uploadDir = WebConfigurationManager.AppSettings["uploadDir"];
            file.SaveAs(Server.MapPath(uploadDir + fileID + fi.Extension));

           //System.Threading.Thread.Sleep(3000);

            //// 回传
            Response.StatusCode = 200; //成功
            return fileID;

            //return Json(new { id = fileID, download = Server.UrlPathEncode(uploadDir + fileID + fi.Extension) });
        }
    }
}
