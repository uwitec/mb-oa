using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity;
using EntityObjectContext;
using System.Linq.Expressions;

namespace EntityObjectLib
{
    [Table("LinkMethods")]
    public class LinkMethod
    {
        [Key]
        public string ID { get; set; }

        [Column("MethodType")]
        public string MethodType { get; set; }

        public string Content { get; set; }

        public string UserID { get; set; }

        [ForeignKey("UserID")] // UserID必须是公共属性！！
        //[InverseProperty("LinkMethods")] // 必须在User中有LinkMethods属性，且是集合类型
        public virtual User user { get; set; }
    }


    //[NotMapped]
    //public static class OriExt
    //{
    //    public static ICollection<LinkMethod> getLinkMethodsByUser(this User user)
    //    {
    //        return null;
    //    }
    //}

    /// <summary>
    /// 如何描述这是一个非实体类
    /// </summary>
    //[NotMapped]
    //public class UserForLinkMethod : User
    //{
    //    public ICollection<LinkMethod> LinkMethods
    //    {
    //        get
    //        {
    //            throw (new NotImplementedException());
    //        }
    //        set
    //        {
    //            throw (new NotImplementedException());
    //        }
    //    }
    //}

    delegate IQueryable<LinkMethod> D2();

    public static class UserForLinkMethod
    {
        public static IQueryable<LinkMethod> LinkMethods(this User u, MyDBExt db)
        {
            return db.LinkMethods.Where(l => l.user.ID.Equals(u.ID));
        }

        //public static Expression LinkMethods(this User u, MyDBExt db)
        //{
        //    Expression<D2> d2 = () => db.LinkMethods.Where(l => l.user.ID == u.ID);
        //    return d2;
        //}
    }
}

