using System;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class 紀錄Action執行時間Attribute : ActionFilterAttribute
    {
        //是是看搭配log4net
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            //filterContext.Controller.ViewData
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //
            TimeSpan executetime = TimeSpan.FromMinutes(17);
            filterContext.Controller.ViewBag.執行時間 = executetime;
            base.OnActionExecuted(filterContext);
        }
    }
}