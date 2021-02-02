using POS.Helper;
using POS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace POS.Controllers
{
    public class HomeController : Controller
    {

        [AuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        // [HttpGet]
        [AuthorizationFilter]
        public ActionResult UserCreate()
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
            string md5StringPassword = AppHelper.GetMd5Hash(password);
            var dataItem = db.Users.Where(x => x.Username == username && x.Password == md5StringPassword).SingleOrDefault();
            bool isLogged = true;
            if (dataItem != null)
            {
                Session["Username"] = dataItem.Username;
                Session["Role"] = dataItem.Role;
                isLogged = true;
            }
            else
            {
                isLogged = false;
            }
            return Json(isLogged, JsonRequestBehavior.AllowGet);
        }
        
           [HttpGet]
           public JsonResult GetAllUser()
        {
            POSEntities db = new POSEntities();
            var dataList = db.Users.Where(x => x.Status == 1).ToList();
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveUser(User user)
        {
            POSEntities db = new POSEntities();
            bool isSuccess = true;
            if (user.UserId > 0)
            {
                db.Entry(user).State = EntityState.Modified;
            }
            else
            {
                user.Status = 1;
                user.Password = AppHelper.GetMd5Hash(user.Password);
                db.Users.Add(user);
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

    }
}