using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    public class EFController : Controller
    {
        FabricsEntities db = new FabricsEntities();
        // GET: EF
        public ActionResult Index()
        {
            //這樣每個函式裡都要寫，所以拉到最外面
            //var db = new FabricsEntities();

            //ProductId,ProductName,Price,Active,Stock
            //db.Product.Add(new Product
            //{
            //    ProductName = "New Beetles",
            //    Price = 50,
            //    Stock = 2,
            //    Active = true
            //});

            Product p = new Product
            {
                ProductName = "New Beetles",
                Price = 50,
                Stock = 2,
                Active = true
            };

            //db.Product.Add(p);
            //db.SaveChanges();

            //這樣可以直接取得PKey
            var pkey = p.ProductId;

            //var plist = db.Product.ToList();
            //var plist = db.Product.ToList().Take(10).OrderBy(pd => pd.ProductId);
            var data1 = db.Product.ToList().OrderByDescending(pd => pd.ProductId).Take(10);

            //Entity Frame Work跟ASP.Net 比起來，批次更新校能很差
            //因為他不是像SQL那樣，是**跑Where條件**，一次更新多筆
            //而是跑迴圈，一次更新一筆資料
            foreach (var item in data1)
            {
                item.Price = item.Price + 2;
            }

            SaveChanges();

            var data = db.Product.ToList().OrderByDescending(pd => pd.ProductId).Take(10);

            return View(data);
        }

        private void SaveChanges()
        {
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    string entityname = item.Entry.Entity.ToString();
                    foreach (DbValidationError err in item.ValidationErrors)
                    {
                        throw new Exception("Entity = " + entityname + ", " + err.ErrorMessage);
                    }
                }
            }
        }

        public ActionResult Detail(int id)
        {
            var product = db.Product.Find(id);

            var data = db.Product.Find(id);
            var data1 = db.Product.Where(pd => pd.ProductId == id).FirstOrDefault();
            var data2 = db.Product.FirstOrDefault(pd => pd.ProductId == id);

            return View(product);
        }

        public ActionResult Delete(int id)
        {
            var product = db.Product.Find(id);

            //若是有關聯，會發生刪除錯誤 (ex: product : order = 1 : N )
            //所以有幾種方法:
            //1.刪除訂單(No Good)
            //foreach (var ol in db.OrderLine.Where(p => p.ProductId ==id).ToList())
            //{
            //    db.OrderLine.Remove(ol);
            //}

            //2. 一筆筆刪除關聯
            foreach (var ol in product.OrderLine.ToList())
            {
                db.OrderLine.Remove(ol);
                ///savechange 不要寫在這裡，這樣會存取多次
                ///可以將資料庫的變動流到最後再寫
                ///類似transaction 的概念
                //db.SaveChanges();
            }

            //3. 第二種方法太麻煩，可以用Remove Range來執行
            //db.Product.RemoveRange(product.OrderLine);(錯誤)
            db.OrderLine.RemoveRange(product.OrderLine);

            db.Product.Remove(product);
            db.SaveChanges();

            //return View();
            return RedirectToAction("Index");
        }
    }
}