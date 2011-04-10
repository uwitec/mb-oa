using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityObjectContext;
using EntityObjectLib;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq.Expressions;

namespace TestProject1
{
    [TestClass]
    public class UT2
    {
        /*
         * 测试扩展的实体上下文
         */
        [TestMethod]
        public void TestMethod1()
        {
            using (MyDBExt db = new MyDBExt())
            {
                //db.Configuration.LazyLoadingEnabled = true;

                //DbSet<LinkMethod> lmss = db.Set<LinkMethod>();
                //lmss.Create();

                //IQueryable<LinkMethod> lmsOld = ext.Set(typeof(LinkMethod));
                // 错误信息：实体类型LinkMethod不是当前上下文db的模型的一部分

                Expression<Func<LinkMethod,LinkMethod>> d2 = p => p;
                IQueryable<LinkMethod> lmsOld = db.LinkMethods.Select(d2);

                // 单向关联，反向查询
                // p.LinkMethods 等价于 db.LinkMethods.Where(l => l.user.ID.Equals(p.ID)) 或者 db.LinkMethods.Where(l => l.user == p)
                //IQueryable<User> test = db.Users.Where(p => db.LinkMethods.Where(l => l.user == p).Count() > 0);
                IQueryable test = db.Users.Select(p => db.LinkMethods.Where(l => l.user == p).Select(l => l) );
                IQueryable test1 = db.Users.Select(p =>new {p.Name,p.Code, p.Roles });
                //IQueryable<User> test = db.Users.Where(p => p.LinkMethods(db).Where(l => l.user == p).Count() > 0);
                 
                //IQueryable test = db.Users.Select(p => p.Roles);
                //IQueryable<LinkMethod> lmsOld = lmss.Select(p => p);
                foreach (LinkMethod lm in lmsOld)
                {
                    db.LinkMethods.Remove(lm);
                    //db.Set<LinkMethod>().Remove(lm);
                }

                //ext.SaveChanges();
                User userOld = db.Users.FirstOrDefault(p => p.Code.Equals("xiongxiong"));
                if (userOld != null)
                {
                    db.Users.Remove(db.Users.FirstOrDefault(p => p.Code.Equals("xiongxiong")));
                }

                User u = new User();
                u.ID = Guid.NewGuid().ToString();
                u.Code = "xiongxiong";
                u.Name = "雄";
                u.Password = "123456";
                db.Users.Add(u);

                LinkMethod lm1 = new LinkMethod();
                lm1.ID = Guid.NewGuid().ToString();
                lm1.MethodType = "A";
                lm1.Content = "content";
                lm1.user = u;

                db.LinkMethods.Add(lm1);
                //db.Set<LinkMethod>().Add(lm1);

                db.SaveChanges();

                IQueryable<LinkMethod> lms = db.Set<LinkMethod>().Select(p => p);
                //IQueryable<User> users = ext.Users.Select(p => p);

                //ICollection<LinkMethod> lms1 = users.Select(p => p.getLinkMethodsByUser());
            }
        }

        //[TestMethod]
        public void TestAttach()
        {
            LinkMethod lm = null;
            using (MyDBExt db = new MyDBExt())
            {
                lm = db.LinkMethods.Include(p=>p.user).FirstOrDefault();
            }

            User user = null;
            using (MyDBExt db = new MyDBExt())
            {
                user = db.Users.FirstOrDefault(u => !u.Code.Equals(lm.user.ID));
            }

            lm.MethodType = "B";
            lm.user = user;

            using (MyDBExt db = new MyDBExt())
            {
                // 附加到新上下文
                // 方法一
                //db.Entry(lm).State = System.Data.EntityState.Modified;
                //db.Entry<User>(lm.user).State = System.Data.EntityState.Modified;
                db.LinkMethods.Add(lm); // lm.user的状态也是Added,如果不把lm.user的状态设置为Unchanged，就会出现主键重复的错误
                db.Entry<LinkMethod>(lm).State = System.Data.EntityState.Modified;
                //db.Entry<User>(lm.user).State = System.Data.EntityState.Unchanged;

                DbReferenceEntry dre = db.Entry<LinkMethod>(lm).Reference(l => l.user);
                bool x = dre.IsLoaded;
                User u = lm.user;
                x = dre.IsLoaded;
                DbEntityEntry<LinkMethod> dee = db.Entry<LinkMethod>(lm);
                DbPropertyEntry dpe = dee.Property(l => l.MethodType);


                //db.LinkMethods.Attach(lm);

                
                //lm.user = user;

                db.SaveChanges();
            }

            using (MyDBExt db = new MyDBExt())
            {
                IQueryable s = db.Users.Select(u => new { u, xX = db.LinkMethods.Where(l => l.user.ID.Equals(u.ID)).Count()});  // OK
                //IQueryable s = db.Users.Select(u => new { u, xX = db.LinkMethods.Where(l => l.user.ID.Equals(u.ID)).Select(l => l.MethodType) });  //NO
                //IQueryable s = db.Users.Select(u => new { u, xX = "ddddddddddddd" }); //OK
            }
        }
    }
}
