using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;

namespace BuizApp.Areas.data.Controllers
{
    public class AddressbookController : Controller
    {
        //
        // GET: /data/Addressbook/

        public ActionResult self()
        {
            string UserID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                IQueryable<EntityObjectLib.AddressBook> result = mydb.AddressBooks.Where(ab => ab.Owner.ID.Equals(UserID)).OrderBy(ab => ab.Name);
                return Json(result.Select(ab => new
                    {
                        ab.ID,
                        ab.Name,
                        ab.Sex,
                        ab.Company,
                        ab.Department,
                        ab.Job,
                        ab.Address,
                        ab.HomePhone,
                        ab.OfficePhone,
                        ab.Mobile,
                        ab.QQ,
                        ab.MSN,
                        ab.Email,
                        ab.BirthDay,
                        ab.Remark,
                        Creator = ab.Creator.Name,
                        ab.CreateTime,
                        Owner = ab.Owner.Name,
                        ab.LastUpdateTime
                    }).ToArray()
                    , JsonRequestBehavior.AllowGet
                    );
            }
        }

        public ActionResult share()
        {
            string UserID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.User user = mydb.Users.Find(UserID);
                IEnumerable<EntityObjectLib.AddressBook> result =
                    user.AddressBookShares.Select(abs => abs.AddressBook)
                    .Union(user.Organization.AddressBookShares.Select(abs => abs.AddressBook))
                    .Distinct();

                return Json(result.Select(ab => new
                    {
                        ab.ID,
                        ab.Name,
                        ab.Sex,
                        ab.Company,
                        ab.Department,
                        ab.Job,
                        ab.Address,
                        ab.HomePhone,
                        ab.OfficePhone,
                        ab.Mobile,
                        ab.QQ,
                        ab.MSN,
                        ab.Email,
                        ab.BirthDay,
                        ab.Remark,
                        Creator = ab.Creator.Name,
                        ab.CreateTime,
                        Owner = ab.Owner.Name,
                        ab.LastUpdateTime
                    }).ToArray()
                    , JsonRequestBehavior.AllowGet
                    );
            }
        }
    }
}
