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
        //public ActionResult Index(bool IsActive = true, string keyword) //給變數預設值
        //public ActionResult Index(bool IsActive = Nullable, string keyword) 錯誤
        public ActionResult Index(bool? IsActive, string keyword)
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

            //Product p = new Product
            //{
            //    ProductName = "New Beetles",
            //    Price = 50,
            //    Stock = 2,
            //    Active = true
            //};

            ////db.Product.Add(p);
            ////db.SaveChanges();

            ////這樣可以直接取得PKey
            //var pkey = p.ProductId;

            //var plist = db.Product.ToList();
            //var plist = db.Product.ToList().Take(10).OrderBy(pd => pd.ProductId);
            //var data1 = db.Product.ToList().OrderByDescending(pd => pd.ProductId).Take(10);

            //Entity Frame Work跟ASP.Net 比起來，批次更新校能很差
            //因為他不是像SQL那樣，是**跑Where條件**，一次更新多筆
            //而是跑迴圈，一次更新一筆資料
            //foreach (var item in data1)
            //{
            //    item.Price = item.Price + 2;
            //}

            //SaveChanges();

            //var data = db.Product
            //    .Where(p => p.Active.HasValue ? p.Active == IsActive : false)
            //    .OrderByDescending(pd => pd.ProductId).Take(10);


            var data = db.Product.OrderByDescending(p => p.ProductId).AsQueryable();

            if (IsActive.HasValue)
            {
                ///如果說在Action那邊設定的變數不為Nullable的Boolean
                ///跟p.Active型別不同
                ///則需要 p.active.value
                ///如果IsActive跟p.Active型別相同，則P.Active即可

                //data = data
                //    .Where(p => p.Active.HasValue ? p.Active == IsActive : false);

                data = data
                    .Where(p => p.Active.HasValue ? p.Active.Value == IsActive : false);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(p => p.ProductName.Contains(keyword));
            }


            return View(data.Take(20));
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
            //記得要加ToList()比較好
            //否則會無法修改迴圈裡的資料，執行時會掛掉
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

        public ActionResult QueryPlan(int num = 5)
        {
            //View 裡面多一欄@item.OrderLine.Count()
            //< td >
            //    @item.OrderLine.Count()
            //</ td >

            ///方法1: 這樣的速度會太慢
            ///每跑一筆資料，就要撈一次 count一次
            ///var data = db.Product.ToList();

            ///方法2 事先載回來 - 弱型別
            //var data = db.Product.Include("OrderLine");

            ///方法3 事先載回來 - 強型別 (最佳)
            //var data1 = db.Product.Include(p => p.OrderLine).AsEnumerable();

            ///執行原始查詢(T-SQL)
            ///db.Database.ExecuteSqlCommand() for Insert, Update, Delete
            ///db.Database.SqlQuery  可以有參數
            var data = db.Database.SqlQuery<Product>(@"
                SELECT p.[ProductId]
                    ,[ProductName]
                    ,[Price]
                    ,[Active]
                    ,[Stock]
                FROM [dbo].[Product] p 
                Left Join [dbo].[OrderLine] o on p.ProductId = o.ProductId
                Where p.ProductId < @P0", num);


            return View(data);


            ///SampleCode From teacher
            /// public ActionResult QueryPlan(int num = 10)
            //{
            //    var data = db.Product
            //        .Include(t => t.OrderLine)
            //        .OrderBy(p => p.ProductId)
            //        .AsQueryable();

            //    //    var data = db.Database.SqlQuery<Product>(@"
            //    //                  SELECT * 
            //    //                  FROM dbo.Product p 
            //    //                  WHERE p.ProductId < @p0", num);

            //    return View(data);
            //}

            ///注意SQL Server Profiler (工具 -> SQL Server Profiler) 可監控SQL Server執行效能
            /
        }
    }
}