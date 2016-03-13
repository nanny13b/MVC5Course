using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MVC5Course.Controllers
{
    [紀錄Action執行時間]
    public class HomeController : BaseController
    {
        [共用ViewBag]
        public ActionResult Index()
        {
            return View();
        }

        [共用ViewBag]
        public ActionResult About()
        {

            // TODO: TEST
            // Cindy: TEST Custom
            //ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}