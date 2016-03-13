using MVC5Course.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC5Course.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        //因為還沒登入，所以要設定可以匿名
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Action(LoginViewModel login)
        {            
            if (CheckLogin(login.Email, login.Password))
            {
                //這句話的意思?
                //false > true 就是記住我的功能
                FormsAuthentication.RedirectFromLoginPage(login.Email, false);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("Password", "認證錯誤，請重新輸入");

            return View();
        }

        private bool CheckLogin(string email, string password)
        {
            //修改從資料庫檢查
            return (email == "nanny13b@hotmail.com" && password == "123");
            throw new NotImplementedException();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return View();
        }
    }
}