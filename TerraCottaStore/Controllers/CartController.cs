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
		
		public async Task<IActionResult>  Add (int id)
        {
            try
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
           
                
                return Ok(new { success = true, message = "Order update successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error has occur");
            }
            TempData["success"] = "Thêm sản phẩm vào giỏ hàng !";
			return Redirect(Request.Headers["Referer"].ToString()) ;
		}
		public IActionResult Checkout()
		{
			
			return View("~/Views/Checkout/Index.cshtml");
		}
		public async Task<IActionResult> Increase(int id)
		{
			List<CartItemModel> Cart = HttpContext.Session.Getjson<List<CartItemModel>>("Cart");
			CartItemModel cartitem = Cart.Where(x => x.ProductID == id).FirstOrDefault();
			cartitem.Quantati += 1;
			HttpContext.Session.Setjson("Cart", Cart);
			TempData["success"] = "Tăng số lượng sản phẩm thành công !";
			return RedirectToAction("index");
		}
		public async Task<IActionResult> Decrease(int id)
		{
			List<CartItemModel> Cart = HttpContext.Session.Getjson<List<CartItemModel>>("Cart");
			CartItemModel cartitem = Cart.Where(x => x.ProductID == id).FirstOrDefault();
			if (cartitem.Quantati > 1)
			{
				--cartitem.Quantati;
			}
			else
			{
				Cart.RemoveAll(x => x.ProductID == id);
			}

			if (Cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.Setjson("Cart", Cart);
			}
			TempData["success"] = "Giảm số lượng sản phẩm thành công !";
			return RedirectToAction("index");
		}
		public async Task<IActionResult> Delete(int id)
		{

			List<CartItemModel> Cart = HttpContext.Session.Getjson<List<CartItemModel>>("Cart");
			CartItemModel cartitem = Cart.Where(x => x.ProductID == id).FirstOrDefault();
			Cart.RemoveAll(x => x.ProductID == id);
			if (Cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.Setjson("Cart", Cart);
			}
			TempData["success"] = "Đã xóa sản phẩm !";
			return RedirectToAction("index");
		}
		public async Task<IActionResult> Clear()
		{
			HttpContext.Session.Remove("Cart");
			TempData["success"] = "Đã Clear giỏ hàng !";
			return RedirectToAction("index");
		}
	}
	
}