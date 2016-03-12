using MVC5Course.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class MBController : Controller
    {
        // GET: MB
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(MemberViewModel data)
        {
            return Content(data.Name + data.Birthday);
        }

        //[HttpPost]
        //public ActionResult Index(string Name, DateTime Birthday)
        //{
        //    return Content(Name + Birthday);
        //}

        //[HttpPost]
        //public ActionResult Index(FormCollection form)
        //{
        //    return Content(form["Name"] + form["Birthday"]);
        //}

        //這種最不好
        //[HttpPost]
        //public ActionResult Index(string a )
        //{
        //    return Content(Request.Form["Name"] + Request.Form["Birthday"]);
        //}
    }
}