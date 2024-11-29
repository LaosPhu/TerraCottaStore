using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _datacontext;

		public CheckoutController(DataContext dataContext)
		{
		_datacontext = dataContext;
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
				_datacontext.SaveChangesAsync();
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
				return RedirectToAction("Index","Cart");
			}
			return View();
		}
	}
}
