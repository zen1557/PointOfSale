using POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace POS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            
            return View();
        }
        public JsonResult CheckLogin(string username, string password)

        {

            POSEntities db = new POSEntities();

            var dataItem = db.Users.Where(x => x.Username == username && x.Password == password).SingleOrDefault();

            bool isLogged = true;

            if (dataItem != null)

            {

                FormsAuthentication.SetAuthCookie(dataItem.Username, true);

                var mdl = System.Web.HttpContext.Current.User.Identity.Name;

                isLogged = true;

            }

            else

            {

                isLogged = false;

            }

            return Json(isLogged, JsonRequestBehavior.AllowGet);

        }

    }
}