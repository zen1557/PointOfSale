﻿using POS.Helper;
using POS.Models;
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
       // [HttpGet]
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
                Session["Username"] = dataItem.Username;
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