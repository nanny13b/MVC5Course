using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC5Course
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            ///如果規則被比對中了，就交由IIS去處理
            ///http://localhost/Home/About
            
            ///step 1. 
            ///{resource} : 路由變數
            ///.axd 字串，表示依定要出現
            ///
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            ///http://localhost/Home/About
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                ///url: "{controller}/{action}/{id}? xx=xxx",  比對規則不會有這種寫法
                ///url: "{controller}/{action}/{id*}",  *只能寫在最後面 ，Home/About/1/2/3/4/5 可以過，因為最後面是*                
                
                /// Google UrlParameter.Optional  codeplex 
                /// Null Object 空物件: 甚麼功能都沒有的物件，原因: 傳來的物件為空值時  + 避免空值時發生Error
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            ///Route有兩種錯誤
            ///1. IIS (localhost/HOME/About/1/1)  Route梅寫好 
            ///2. MVC, ASPX等.NET錯誤  (localhost/HOME/About2/2)
            ///兩種畫面不同，可試試看
        }
    }
}
