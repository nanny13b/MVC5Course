using System;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class 共用ViewBagAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Message = "共用資料";

            //可以在這邊做判斷，如果不符合條件A，則丟一個新的Result，就不會進入原本的Result
            //if (!filterContext.HttpContext.Request.IsLocal)
            //{
            //    filterContext.Result = new RedirectResult("/");
            //}

            base.OnActionExecuting(filterContext);
        }
    }
}