using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityObjectContext;
using System.Linq.Dynamic;
using EntityObjectLib;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Data.Entity;


namespace TestProject1
{
    [TestClass]
    public class EFLinq
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (MyDB mydb = new MyDB())
            {
                var s = mydb.Privileges.GroupJoin(
                    mydb.RolePrivileges.Where(rp => rp.Role.roleCode == "normal user"),
                    p => p.ID,
                    rp => rp.Privilege.ID,
                    (p, rp) => new { p.ID, p.privilegeCode, p.privilegeName, p.orderNO, has = rp.Count() > 0 }
                        );
                return;
            }
        }

        [TestMethod]
        public void DynQuery()
        {
            //string queryString = "{代码}.Contains(\"l\") and {名称}.Contains(\"伟\")";  //"Code.Contains(\"l\") and Name.Contains(\"伟\")"
            string queryString = "{代码}==\"lilin\"";

            //string queryString = "1==1";

            Type t = typeof(User);
            PropertyInfo[] PropertyInfos = t.GetProperties().ToArray();
            foreach (PropertyInfo pi in PropertyInfos)
            {
                DisplayAttribute[] attrs = pi.GetCustomAttributes(typeof(DisplayAttribute), true) as DisplayAttribute[];
                if (attrs.Length > 0)
                {
                    //Console.WriteLine(pi.Name);
                    string Name = attrs[0].Name;

                    queryString = queryString.Replace(
                        string.Format("{{{0}}}", Name),
                        pi.Name
                        );
                }
            }

            using (MyDB mydb = new MyDB())
            {
                IQueryable<User> users = mydb.Users.Where(queryString);
            }
        }

        [TestMethod]
        public void TestExpression()
        {
            Expression<Func<User, bool>> expr1 = u => u.Code.Contains("l");
            Expression<Func<User, bool>> expr2 = u => u.Code.Contains("x");
            //Expression<Func<User, bool>> expr = (u) => expr1.Compile()(u) && expr2.Compile()(u); //The parameter 'u' was not bound in the specified LINQ to Entities query expression.
            //ParameterExpression pe = Expression.Parameter(typeof(User), "u");
            //ParameterExpression p = expr1.Parameters.Single();
            //Expression<Func<User, bool>> expr = expr1.compo

            using (MyDB mydb = new MyDB())
            {
                //IQueryable<User> users = mydb.Users.Where(expr);
                IQueryable<User> users = mydb.Users.Where(expr1).Where(expr2);
            }
        }

        [TestMethod]
        public void testExpressionBase()
        {
            // public delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2); //委托类型Func是在System命名空间中为我们定义好的
            Func<int, int, int> func1 = (a, b) => a + b; //可执行代码
            //Expression<Func<int, int, int>> expression1 = func1; //可执行代码转换为表达式???

            Expression<Func<int, int, int>> expression2 = (a, b) => a + b; //创建表达式
            Func<int, int, int> func2 = expression2.Compile(); //表达式转换为可执行代码

            return;
        }

        [TestMethod]
        public void lazy()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.Configuration.LazyLoadingEnabled = false;
                Organization org = mydb.Organizations.First();
                mydb.Users.Load();
                User[] users = org.Users.ToArray();
            }
        }

        [TestMethod]
        public void exeSQL()
        {
            using (MyDB mydb = new MyDB())
            {
                User u = mydb.Database.SqlQuery<User>("select s.*,u.* from users u left join subjects s on u.id=s.id").First();
            }
        }

        [TestMethod]
        public void getSQL()
        {
            using (MyDB mydb = new MyDB())
            {
                IQueryable<User> users = mydb.Users.Where(u => u.Code.Contains("lx"));
                string sql = users.ToString();
            }
        }

        [TestMethod]
        public void sss()
        {
            using (MyDB mydb = new MyDB())
            {
                User user = mydb.Users.First(u => u.Code.Contains("lx"));
                mydb.Entry(user).Collection(u => u.CreateWFTemplates).Query().Load();
            }
        }

        //http://www.cnblogs.com/LingzhiSun/

        [TestMethod]
        public void rrr()
        {
            MyDB mydb = new MyDB();
            mydb.Users.Load();
            IEnumerable<User> us2 = mydb.Users.Local.Where(u => 1==1);
            
            User user1 = mydb.Users.First(u => u.Code.Contains("lxx"));
            user1.Name = "asdfasdfasdf";
            User nu = new User
            {
                ID = Guid.NewGuid().ToString(),
                Code = "lxx"
            };
            mydb.Users.Add(nu);

            MyDB mydb1 = new MyDB();
            User user3 = mydb1.Users.First(u => u.Code.Contains("lxx"));
            user3.MSN = "wwwwwwwwwwwwww";

            User user2 = mydb.Users.First(u => u.Code.Contains("lxx"));
            user2.MSN = "ssssssssssssssss";
            //mydb.Users.Remove(user2);

            IEnumerable<User> us1 = mydb.Users.Where(u => 1==1);

            

        }
    }
}
