using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TerraCottaStore.Areas.Admin.Repository;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _datacontext;
        private readonly IEmailSender _emailSender;
        public CheckoutController(IEmailSender emailSender,DataContext dataContext)
		{
			_datacontext = dataContext;
			 _emailSender = emailSender;

        }
		public async Task<IActionResult> Checkout ()
		{	var userEmail =User.FindFirstValue(ClaimTypes.Email);
			if (string.IsNullOrWhiteSpace(userEmail))
			{ return RedirectToAction("Login", "Account"); }
			else
			{
				var ordercode =Guid.NewGuid().ToString();
				var orderitem = new OrderModel();
				orderitem.OrderCode = ordercode;
				orderitem.UserName = userEmail;
				orderitem.Status = 1;
				orderitem.CreatedDate = DateTime.Now;
				_datacontext.Add(orderitem);
				await _datacontext.SaveChangesAsync();
				List <CartItemModel> cartItems = HttpContext.Session.Getjson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach (var cart in cartItems)
				{
					var orederdetail = new OrderDetails();
					orederdetail.UserName =userEmail;
					orederdetail.OrderCode =ordercode;
					orederdetail.ProductId = cart.ProductID;
					orederdetail.Quantati  = cart.Quantati;
					orederdetail.Price     = cart.Price;
					_datacontext.Add(orederdetail);
					_datacontext.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");
				TempData["success"] = "Tao don hang thanh cong";
				emailpos();
                return RedirectToAction("Index","Cart");
			}
			return View();
		}
		public async void emailpos ()
		{
            var receiver = "nhuhanlam@gmail.com";
            var subject = "Thinh dep trai";
            var Message = "test sever";
            await _emailSender.SendEmailAsync(receiver, subject, Message);
        }
	}
}
