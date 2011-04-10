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
    public class DbPathProvider : VirtualPathProvider
    {
        public DbPathProvider()
            : base()
        {

        }

        /// <summary>
        /// 判断指定路径的文件是否存在
        /// </summary>
        /// <param name="virtualPath">指定路径</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public override bool FileExists(string virtualPath)
        {
            //Database db = new Database();
            //return db.Views.Any(w => w.Path == virtualPath);

            throw (new Exception());
        }

        /// <summary>
        /// 获得指定路径的文件
        /// </summary>
        /// <param name="virtualPath">指定路径</param>
        /// <returns>文件</returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            //return new DbVirtualFile(virtualPath);
            throw (new Exception());
        }
    }
}