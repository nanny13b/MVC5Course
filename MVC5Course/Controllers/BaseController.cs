using MVC5Course.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    //在BaseController 這邊套用，表示所有的Controller都要認證(Login)過後才能登入，就不需要在每頁都寫重覆的驗證Code
    //[Authorize]  
    public class BaseController : Controller
    {
        ProductRepository repo = RepositoryHelper.GetProductRepository();

        //protected override void HandleUnknownAction(string actionName)
        //{
        //    this.Redirect("/").ExecuteResult(this.ControllerContext);
        //    //base.HandleUnknownAction(actionName); 預設是傳回404
        //}

        
        // GET: Base
        public ActionResult Debug()
        {
            //return View();
            return Content("DeBug");
        }
    }
}