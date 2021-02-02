using System;
using System.Collections.Generic;
using System.Linq;
using POS.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace POS.Helper
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            POSEntities db = new POSEntities();
            string username = Convert.ToString(System.Web.HttpContext.Current.Session["Username"]);
            string role = Convert.ToString(System.Web.HttpContext.Current.Session["Role"]);
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string tag = controllerName + actionName;

            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                // Don't check for authorization as AllowAnonymous filter is applied to the action or controller
                return;
            }

            // Check for authorization
            if (System.Web.HttpContext.Current.Session["Username"] == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            if (username != null && username != "")
            {
                bool isPermitted = false;

                var viewPermission = db.RolePermissions.Where(x => x.Role == role && x.Tag == tag).SingleOrDefault();
                if (viewPermission != null)
                {
                    isPermitted = true;
                }
                if (isPermitted == false)
                {
                    filterContext.Result = new RedirectToRouteResult(
                      new RouteValueDictionary
                        {
                             { "controller", "Home" },
                             { "action", "AccessDenied" }
                        });
                }
            }
        }
    }
}
 