using MVC5Course.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class ARController : BaseController
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


        //Cindy: 不成功 console.log沒有資料
        public ActionResult JsonTest()
        {
            ProductRepository repo = RepositoryHelper.GetProductRepository();
            repo.UnitOfWork.Context.Configuration.LazyLoadingEnabled = false;
            var data = repo.All().Take(3);

            return Json(data, JsonRequestBehavior.AllowGet);
            //return View();
        }

    }
}