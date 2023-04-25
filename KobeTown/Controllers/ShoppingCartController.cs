using KobeTown.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KobeTown.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
			List<CartItem> ShoppingCart = GetShoppingCartFromSession();
			if (ShoppingCart.Count == 0)
				return RedirectToAction("Index", "Products");
			ViewBag.Tongsoluong = ShoppingCart.Sum(p => p.Quantity);
			ViewBag.Tongtien = ShoppingCart.Sum(p => p.Quantity * p.Price);
			return View(ShoppingCart);
        }
		public List<CartItem> GetShoppingCartFromSession()
		{
			var lstShoppingCart = Session["ShoppingCart"] as List<CartItem>;
			if (lstShoppingCart == null)
			{
				lstShoppingCart = new List<CartItem>();
				Session["ShoppingCart"] = lstShoppingCart;
			}
			return lstShoppingCart;
		}
		public RedirectToRouteResult AddToCart(int id)
        {
            ProductModelContext db = new ProductModelContext();
            List<CartItem> ShoppingCart = GetShoppingCartFromSession();
            CartItem findCartItem = ShoppingCart.FirstOrDefault(m => m.Id == id);
            if (findCartItem == null)
            {
                Products findproduct = db.Productss.FirstOrDefault(m => m.Id == id);
                CartItem newItem = new CartItem()
                {
                    Id = findproduct.Id,
                    Description = findproduct.Description,
                    Quantity = 1,
                    Image = findproduct.Image,
                    Price = findproduct.Price,
					Category = findproduct.Category,
					Name = findproduct.Name
                };
                ShoppingCart.Add(newItem);
            }
            else
            {
                findCartItem.Quantity++;
            }
			//return RedirectToAction("Index", "ShoppingCart");
			return RedirectToAction("Index", "Products");
		}
		public RedirectToRouteResult BuyNow(int id)
		{
			ProductModelContext db = new ProductModelContext();
			List<CartItem> ShoppingCart = GetShoppingCartFromSession();
			CartItem findCartItem = ShoppingCart.FirstOrDefault(m => m.Id == id);
			if (findCartItem == null)
			{
				Products findproduct = db.Productss.FirstOrDefault(m => m.Id == id);
				CartItem newItem = new CartItem()
				{
					Id = findproduct.Id,
					Description = findproduct.Description,
					Quantity = 1,
					Image = findproduct.Image,
					Price = findproduct.Price,
					Category = findproduct.Category,
					Name = findproduct.Name
				};
				ShoppingCart.Add(newItem);
			}
			else
			{
				findCartItem.Quantity++;
			}
			return RedirectToAction("Index", "ShoppingCart");
		}
		public RedirectToRouteResult UpdateCart(int id, int txtQuantity)
		{
			var itemFind = GetShoppingCartFromSession().FirstOrDefault(p => p.Id == id);
			if (itemFind != null)
			{
				itemFind.Quantity = txtQuantity;
			}
			return RedirectToAction("Index");
		}
		public ActionResult CartSummary()
        {
			List<CartItem> ShoppingCart = GetShoppingCartFromSession();
			int count = 0;
			foreach (CartItem cartItem in ShoppingCart) { 
				count = count	+ cartItem.Quantity;
			}
			ViewBag.CartCount = count;
			return PartialView("CartSummary");
        }
		//------------------------------------------------------------------------
		public ActionResult Orderr()
		{
			string currentUserId = User.Identity.GetUserId();
			ProductModelContext context = new ProductModelContext();
			using (DbContextTransaction transaction = context.Database.BeginTransaction())
			{
				try
				{
					Orderr objOrder = new Orderr();
					objOrder.CustomerId = currentUserId;
					objOrder.OrderDate = DateTime.Now;
					objOrder.DeliveryDate = null;
					objOrder.isComplete = false;
					objOrder.isPaid = false;
					context.Orderrs.Add(objOrder);

					context.SaveChanges();
					int newOrderNo = objOrder.OrderNo;

					List<CartItem> listCartItems = GetShoppingCartFromSession();
					foreach (var item in listCartItems)
					{
						OrderDetaill ctdh = new OrderDetaill()
						{
							OrderNo = newOrderNo,
							ProductId = item.Id,
							Quantity = item.Quantity,
							Price = item.Price,
						};
						context.OrderDetaills.Add(ctdh);
						context.SaveChanges();
					}
					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					return Content("Gặp lỗi khi nhận hàng");
				}
				Session["Giohang"] = null;
			}
			return RedirectToAction("ConfirmOrder", "ShoppingCart");
		}
		public ActionResult ConfirmOrder()
		{
			return View();
		}
		public RedirectToRouteResult RemoveCartItem(int id)
		{
			var itemFind = GetShoppingCartFromSession().FirstOrDefault(p => p.Id == id);
			GetShoppingCartFromSession().Remove(itemFind);
			return RedirectToAction("Index");
		}
		public RedirectToRouteResult Delete()
		{
			GetShoppingCartFromSession().Clear();
			return RedirectToAction("Index");
		}
	}
}