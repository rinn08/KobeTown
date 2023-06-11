﻿using KobeTown.Models;
using KobeTown.Others;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace KobeTown.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
		public ActionResult Payment()
		{
			//List<Book> books = Session["Cart"] as List<Book> ;
			//request params need to request to MoMo system
			string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
			string partnerCode = "MOMOOJOI20210710";
			string accessKey = "iPXneGmrJH0G8FOP";
			string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
			string orderInfo = "test";
			string returnUrl = "https://localhost:44398/Home/ConfirmPaymentClient";
			string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

			string amount = "1000";
			string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
			string requestId = DateTime.Now.Ticks.ToString();
			string extraData = "";

			//Before sign HMAC SHA256 signature
			string rawHash = "partnerCode=" +
				partnerCode + "&accessKey=" +
				accessKey + "&requestId=" +
				requestId + "&amount=" +
				amount + "&orderId=" +
				orderid + "&orderInfo=" +
				orderInfo + "&returnUrl=" +
				returnUrl + "&notifyUrl=" +
				notifyurl + "&extraData=" +
				extraData;

			MoMoSecurity crypto = new MoMoSecurity();
			//sign signature SHA256
			string signature = crypto.signSHA256(rawHash, serectkey);

			//build body json request
			JObject message = new JObject
			{
				{ "partnerCode", partnerCode },
				{ "accessKey", accessKey },
				{ "requestId", requestId },
				{ "amount", amount },
				{ "orderId", orderid },
				{ "orderInfo", orderInfo },
				{ "returnUrl", returnUrl },
				{ "notifyUrl", notifyurl },
				{ "extraData", extraData },
				{ "requestType", "captureMoMoWallet" },
				{ "signature", signature }

			};

			string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

			JObject jmessage = JObject.Parse(responseFromMomo);

			return Redirect(jmessage.GetValue("payUrl").ToString());
		}

		private static List<Orderr> orderList = new List<Orderr>();

		// Hàm lấy thông tin đơn hàng từ cơ sở dữ liệu
		private Orderr GetOrderFromDatabase(string orderId)
		{
			int OrderNo = 0;
			return orderList.FirstOrDefault(o => o.OrderNo == OrderNo);
		}

		// Hàm lưu thông tin đơn hàng vào cơ sở dữ liệu
		private void SaveOrderToDatabase(Orderr order)
		{
			Orderr existingOrder = orderList.FirstOrDefault(o => o.OrderNo == order.OrderNo);
			if (existingOrder != null)
			{
				existingOrder.isPaid = order.isPaid;
				existingOrder.isComplete = order.isComplete;
			}
			else
			{
				orderList.Add(order);
			}
		}



		//Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
		//errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
		//Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
		public ActionResult ConfirmPaymentClient(Result result)
		{
			//lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
			string rMessage = result.message;
			string rOrderId = result.orderId;
			string rErrorCode = result.errorCode; // = 0: thanh toán thành công
			if (rErrorCode == "0")
			{
				// Đơn hàng thành công, cập nhật biến isPay và isComplete thành true
				// Tùy theo cách bạn lưu trữ dữ liệu, bạn có thể cập nhật giá trị này vào đâu (ví dụ: cập nhật vào cơ sở dữ liệu, session, cache, ...)
				// Ví dụ: cập nhật biến isPay và isComplete trong đối tượng Order
				Orderr order = GetOrderFromDatabase(rOrderId); // Lấy thông tin đơn hàng từ cơ sở dữ liệu (hàm GetOrderFromDatabase là giả định)
				if (order != null)
				{
					order.isPaid = true;
					order.isComplete = true;
					SaveOrderToDatabase(order); // Lưu thông tin đơn hàng đã cập nhật vào cơ sở dữ liệu (hàm SaveOrderToDatabase là giả định)
				}
			}
			return View();
		}

		[HttpPost]
		public void SavePayment()
		{
			//cập nhật dữ liệu vào db
			String a = "";
		}
	}
}
