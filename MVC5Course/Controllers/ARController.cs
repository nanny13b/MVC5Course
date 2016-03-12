using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class ARController : Controller
    {
        // GET: AR
        public ActionResult Index()
        {
            return View();
        }

        // GET: AR
        public ActionResult Index2()
        {
            return PartialView("Index");
        }

        public ContentResult ContentTest()
        {
            return Content("<script>alert('redirecting.....');</script>", "application/javascript", Encoding.UTF8);

        }

        public FileResult FileTest()
        {
            return File(Server.MapPath("~/Content/yoga.jpg"), "image/jpeg");
        }
    }
}