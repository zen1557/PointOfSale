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
        public ActionResult Logout()
        {
            Session["Username"] = null;
            Session["Role"] = null;
            return RedirectToAction("Login");
        }
        [AuthorizationFilter]
        public ActionResult Category()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveCategory(Category cat)
        {
            POSEntities db = new POSEntities();
            bool isSuccess = true;
            if (cat.CategoryId > 0)
            {
                db.Entry(cat).State = EntityState.Modified;
            }
            else
            {
                cat.Status = 1;
                db.Categories.Add(cat);
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
        [HttpGet]
        public JsonResult GetAllGetegory()
        {
            POSEntities db = new POSEntities();
            var dataList = db.Categories.Where(x => x.Status == 1).ToList();
            var data = dataList.Select(x => new {
                CategoryId = x.CategoryId,
                Name = x.Name,
                Status = x.Status
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [AuthorizationFilter]
        public ActionResult Product()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveProduct(Product product)
        {
            POSEntities db = new POSEntities();
            bool isSuccess = true;
             if (product.ProductId > 0)
            {
                db.Entry(product).State = EntityState.Modified;
            }
            else
            {
                db.Products.Add(product);
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
        [HttpGet]
        public JsonResult GetAllProduct()
        {
            POSEntities db = new POSEntities();
            var dataList = db.Products.Include("Category").ToList();
            var modefiedData = dataList.Select(x => new
            {
                ProductId = x.ProductId,
                CategoryId = x.CategoryId,
                Name = x.Name,
                Status = x.Status,
                CategoryName = x.Category.Name
            }).ToList();
            return Json(modefiedData, JsonRequestBehavior.AllowGet);
        }
        [AuthorizationFilter]
        public ActionResult Batch()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveBatch(Batch batch)
        {
            POS.Helper.AppHelper.ReturnMessage retMessage = new AppHelper.ReturnMessage();
            POSEntities db = new POSEntities();
            retMessage.IsSuccess = true;

            if (batch.BatchId > 0)
            {
                db.Entry(batch).State = EntityState.Modified;
                retMessage.Messagae = "Update Success!";
            }
            else
            {
                batch.BatchName = batch.BatchName + db.Batches.Count();
                var batchData = db.Batches.Where(x => x.BatchName.Equals(batch.BatchName)).SingleOrDefault();
                if (batchData == null)
                {
                    db.Batches.Add(batch);
                    retMessage.Messagae = "Save Success!";
                }
                else
                {
                    retMessage.IsSuccess = false;
                    retMessage.Messagae = "This batch already exist!Please refresh and again try!";
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                retMessage.IsSuccess = false;
            }

            return Json(retMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllBatch()
        {
            POSEntities db = new POSEntities();
            var dataList = db.Batches.ToList();
            var modefiedData = dataList.Select(x => new
            {
                BatchId = x.BatchId,
                BatchName = x.BatchName,
            }).ToList();
            return Json(modefiedData, JsonRequestBehavior.AllowGet);
        }


    }
}