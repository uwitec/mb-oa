using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace BuizApp.RazorByDB
{
    /// <summary>
    /// ASP.NET MVC load Razor view from database:
    /// http://stackoverflow.com/questions/4218454/asp-net-mvc-load-razor-view-from-database
    /// 数据库示例：
    /// 表名：Views
    /// 列Path:路径
    ///  未完成
    /// </summary>
    public class DbVirtualFile : VirtualFile
    {

        public DbVirtualFile(string path)
            : base(path)
        {

        }

        /// <summary>
        /// 打开文件流
        /// </summary>
        /// <returns></returns>
        public override System.IO.Stream Open()
        {
        //    Database db = new Database();
        //    return new System.IO.MemoryStream(
        //               db.Views.Single(v => v.Path == this.VirtualPath));
            throw (new Exception());
        }
    }
}