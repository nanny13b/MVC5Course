using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    public class ProductsController : BaseController
    {
        /// <summary>
        /// 產生 Repository的方法，下載IRepository.EF6.tt，拖曳到Models， 拉到Fabrics.edmx
        /// 就會自動產生一堆code
        /// </summary>
        //private FabricsEntities db = new FabricsEntities();
        ProductRepository repo = RepositoryHelper.GetProductRepository();

        // GET: Products
        //public ActionResult Index()
        //{
        //    // var data = db.Product.OrderByDescending(p => p.ProductId).Take(10);
        //    var data = repo.All().OrderByDescending(p => p.ProductId).Take(5);
        //    return View(data);
        //}


        //03/19 下拉選單 (dropdownlist vs modelbinding)
        //1. View 只要一行 Dropdown的名稱
        //2. 給訂VIewDAta的資料 (ModelBinding)
        //3. Model (預設值 從 ModelState帶過來)
        public ActionResult Index(int? PID, string type, bool? IsActive, string keyword)
        {
            var data = repo.All(true);

            #region 下拉選單

            if (IsActive.HasValue)
            {
                data = data.Where(p => p.Active.HasValue && p.Active == IsActive.Value).Take(5);
            }

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "有效", Value = "true" });
            items.Add(new SelectListItem { Text = "無效", Value = "false" });

            ViewData["IsActive"] = new SelectList(items, "Value", "Text");

            #endregion

            #region 搜尋
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.Where(p => p.ProductName.Contains(keyword));
            }
            #endregion

            if (PID.HasValue)
            {
                ViewBag.SelectedProductID = PID.Value;
                ViewBag.type = type;
            }
            //return View(); //不需要再View裡面 傳入ViewBag，因為本來就會帶入ViewBag跟ViewData
            return View(data);
        }        

       [HttpPost]
        public ActionResult Index(IList<Product批次更新ViewModel> data)
        {
            //只要有Binding 就有驗證資料
            if (ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    var p = repo.Find(item.ProductId);
                    p.Price = item.Price;
                    p.Stock = item.Stock;
                }
                repo.UnitOfWork.Commit();
            }

            //回到首頁
            //Cindy: 這樣不行，(如果直接丟View，產品名稱消失了)
            //return View(data); (錯誤版)
            //return RedirectToAction("Index"); (正確版)

            return View(repo.All().OrderByDescending(p => p.ProductId).Take(5));
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Product product = db.Product.Find(id);
            Product product = repo.Find(id.Value);
            if (product == null && !product.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                //db.Product.Add(product);
                repo.Add(product);

                #region SaveChange vs UnitOfWork.Commit()
                //db.SaveChanges();
                //unitofWork可以做 db可以做的事情 所以原本的savechanges就改成呼叫UnitOfWork.Commit()
                ///public void Commit()
                ///{
                ///    Context.SaveChanges();
                ///} 
                #endregion
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repo.Find(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        //public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)(錯誤版)
        //Bind: Model.IsValid 預先驗證
        // Step 1 先找到Action(Edit)
        // Step 2  找到參數production，
        // Step 3 看哪些資料要被Bind 再開始比對
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)(錯誤版)
        public ActionResult Edit(int id, FormCollection form) //不會用到form，放這個的原因是為了跟上面那個Action作分別
        {
            Product p = repo.Find(id);

            //modelbinding 仔細看那邊篇老師的文章

            //if (ModelState.IsValid)
            if(TryUpdateModel<Product>(p, new string[] { "ProductId,ProductName,Price,Active,Stock" }))  //延遲驗證：Action開始後，這邊才開始做model binding ，可以額外加參數 ex: Include Property。可控性比較高
            //if(TryUpdateModel<IProduct>(p)) //1.增加一個介面 可以自訂驗證 也可以控制只要用到哪些欄位 //2.可以用F2修改全站欄位名稱
            {
                ///這個寫法很不好，可以任意修改Product的資料
                ///以及沒有Include ProductID
                /*
                var db = (FabricsEntities)repo.UnitOfWork.Context;

                //這個product不是從資料庫撈出來的 ex: Find
                //所以從資料庫抓到這筆資料，將他的狀態設定為可修改
                db.Entry(product).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                */
                repo.UnitOfWork.Commit();

                TempData["EditMessage"] = "商品資料更新成功";

                return RedirectToAction("Index");
            }
            return View(p);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repo.Find(id.Value);
            //db.Product.Remove(product);
            //db.SaveChanges()
            product.IsDeleted = true;
            repo.UnitOfWork.Commit();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = repo.Find(id);
            repo.Delete(product);
            //db.Product.Remove(product);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var db = (FabricsEntities)repo.UnitOfWork.Context;
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //public ActionResult GetOrderLines(int pid)
        //{
        //    List<OrderLine> olist = repo.Find(pid).OrderLine.ToList();
        //    ViewBag.OrderLines = olist;
        //    return View(ViewBag);
        //}
    }
}
