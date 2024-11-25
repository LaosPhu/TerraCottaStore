using Microsoft.AspNetCore.Mvc;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Controllers
{
	public class CartController : Controller
	{
		private readonly DataContext _datacontext;

		public CartController(DataContext dataContext) 
		{
		 _datacontext = dataContext;
		}
		public IActionResult index()
		{
			List<CartItemModel> Items = HttpContext.Session.Getjson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemViewModel CartVM = new()
			{
				CartItems = Items,
				GrandTotal = Items.Sum(a => a.Quantati * a.Price ),

			};
			return View(CartVM);
		}	
		public IActionResult Checkout()
		{
			return View("~/Views/Checkout/Index.cshtml");
		}
		public async Task<IActionResult>  Add (int id)
		{
			ProductModel product =await _datacontext.Products.FindAsync(id);
			List <CartItemModel> Cart = HttpContext.Session.Getjson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItem = Cart.Where( x => x.ProductID == id).FirstOrDefault();
			if (cartItem == null)
			{
				Cart.Add(new CartItemModel(product));
			}
			else
			{
				cartItem.Quantati += 1;
			}
			HttpContext.Session.Setjson("Cart",Cart);
			return Redirect(Request.Headers["Referer"].ToString()) ;
		}
	}

}