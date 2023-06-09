﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KobeTown.Models;
using PagedList.Mvc;
using PagedList;

namespace KobeTown.Areas.Admin.Controllers
{
    public class ProductsAdmController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Products
        [Authorize(Roles ="Admin")]
		public ActionResult Index(int? page)
		{
			var products = db.Products.OrderBy(p => p.Name);
			int pageSize = 6;
			int pageNumber = (page ?? 1);
			return View(products.ToPagedList(pageNumber, pageSize));
		}
		[Authorize(Roles = "Admin")]
		public ActionResult IndexSearch(int? page)
		{
			var products = db.Products.OrderBy(p => p.Name);
			int pageSize = 6;
			int pageNumber = (page ?? 1);
			return View(products.ToPagedList(pageNumber, pageSize));
		}
		[Authorize(Roles = "Admin")]
		public ActionResult GuitarAdm()
		{
			return View(db.Products.ToList());
		}
		[Authorize(Roles = "Admin")]
		public ActionResult PianoAdm()
		{
			return View(db.Products.ToList());
		}
		[Authorize(Roles = "Admin")]
		public ActionResult DrumAdm()
		{
			return View(db.Products.ToList());
		}
		[Authorize(Roles = "Admin")]
		public ActionResult GearAdm()
		{
			return View(db.Products.ToList());
		}
		// GET: Admin/Products/Details/5
		public ActionResult DetailsAdm(int? id)
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

        // GET: Admin/Products/Create
        public ActionResult CreateAdm()
        {
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdm([Bind(Include = "Id,Name,Category,Status,Description,Price,Image")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult EditAdm(int? id)
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

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdm([Bind(Include = "Id,Name,Category,Status,Description,Price,Image")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(products);
        }
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult CreateAdm([Bind(Include = "Id,Name,Category,Status,Description,Price,Image")] Products products)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		db.Products.Add(products);
		//		db.SaveChanges();
		//		return RedirectToAction("Index");
		//	}

		//	return View(products);
		//}


		// GET: Admin/Products/Delete/5

		// GET: Admin/Products/Delete/5
		public ActionResult DeleteAdm(int? id)
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

		// POST: Admin/Products/Delete/5
		[HttpPost, ActionName("DeleteAdm")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmedAdm(int id)
		{
			Products products = db.Products.Find(id);
			if (products == null)
			{
				return HttpNotFound();
			}
			db.Products.Remove(products);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult SearchAdm(string searchString, int? page)
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
            if (searchString == null) return RedirectToAction("Index");
			return View("IndexSearch", results.ToPagedList(pageNumber, pageSize));
		}
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
