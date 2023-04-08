using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KobeTown.Models;
using PagedList.Mvc;
using PagedList;

namespace KobeTown.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Products
		public ActionResult Index(int? page)
		{
			var products = db.Products.OrderBy(p => p.Name);
			int pageSize = 6;
			int pageNumber = (page ?? 1);
			return View(products.ToPagedList(pageNumber, pageSize));
		}
		public ActionResult Guitar()
        {
            return View(db.Products.ToList());
        }
		public ActionResult Piano()
		{
			return View(db.Products.ToList());
		}
		public ActionResult Drum()
		{
			return View(db.Products.ToList());
		}
		public ActionResult Gear()
		{
			return View(db.Products.ToList());
		}

		// GET: Products/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Category,Status,Description,Price,Image")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Category,Status,Description,Price,Image")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
		public ActionResult Search(string searchString, int? page)
		{
			var context = new ApplicationDbContext();
			var results =
				(from m in context.Products
				 where
				 m.Name.Contains(searchString)
				 || m.Category.Contains(searchString)
				 select m).OrderBy(p => p.Name);
			int pageSize = 6;
			int pageNumber = (page ?? 1);
			return View("Index", results.ToPagedList(pageNumber, pageSize));
		}

		public ActionResult SearchAjax(string keySearch = "")
		{
			var products = db.Products.Where(x => x.Name.ToLower().Contains(keySearch.ToLower())).ToList();  
			
			return PartialView("products");
		}
		//---------------------------------------
		
		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
