using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;

namespace BuizApp.Areas.data.Controllers
{
    public class RSSController : Controller
    {
        //
        // GET: /data/RSS/

        public ActionResult Index()
        {
            string rss = Request.Params["rss"];
            WebRequest request = WebRequest.Create(rss);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Response.ContentType = "application/xml";
            StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
            Response.Write(reader.ReadToEnd());
            return null;
        }

    }
}
