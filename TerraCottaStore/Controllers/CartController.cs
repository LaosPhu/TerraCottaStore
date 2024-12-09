using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
		public IActionResult Index()
		{ 
			List<CartItemModel> Items = HttpContext.Session.Getjson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemViewModel CartVM = new()
			{
				CartItems = Items,
				GrandTotal = Items.Sum(a => a.Quantati * a.Price ),

			};
			return View(CartVM);
		}
		[HttpPost]
		public async Task<IActionResult>  Add (int id)
        {
            try
            {
                ProductModel product =await _datacontext.Products.FindAsync(id);
			List <CartItemModel> Cart = HttpContext.Session.Getjson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItem = Cart.Where( x => x.ProductID == id).FirstOrDefault();
			if (cartItem == null)
			{
					product.Quantity = 1;
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
		[HttpPost]
		public async Task<IActionResult> Addmanydetail(int Id,int Quantity)
		{
			try
			{
				ProductModel product = await _datacontext.Products.FindAsync(Id);
				
				List<CartItemModel> Cart = HttpContext.Session.Getjson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				CartItemModel cartItem = Cart.Where(x => x.ProductID == Id).FirstOrDefault();
				if (cartItem == null)
				{	product.Quantity = Quantity;
					Cart.Add(new CartItemModel(product));
				}
				else
				{
					cartItem.Quantati += Quantity;
				}
				HttpContext.Session.Setjson("Cart", Cart);


				return Ok(new { success = true, message = "Order update successfully" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error has occur");
			}
			TempData["success"] = "Thêm sản phẩm vào giỏ hàng !";
			return Redirect(Request.Headers["Referer"].ToString());
		}
		public IActionResult Checkout()
		{
			
			return View("~/Views/Checkout/Index.cshtml");
		}
		[HttpPost]
		public async Task<IActionResult> Increase(int id)
		{	
			List<CartItemModel> Cart = HttpContext.Session.Getjson<List<CartItemModel>>("Cart");
			CartItemModel cartitem = Cart.Where(x => x.ProductID == id).FirstOrDefault();
			var productmodel = await _datacontext.Products.FirstOrDefaultAsync(x =>x.Id == id);
			if (productmodel.Quantity > cartitem.Quantati)
			{
				cartitem.Quantati += 1;

				HttpContext.Session.Setjson("Cart", Cart);
		
				return Ok(new { success= true, quantity = cartitem.Quantati, productId= id});
				return RedirectToAction("index");
			}
			
			else
			{
				return StatusCode(500, "An error has occur");
				TempData["error"] = "Số lượng sản phẩm nhiều hơn kho !";
				return RedirectToAction("index");

			}
		}
		[HttpPost]
		public async Task<IActionResult> Decrease(int id)
		{
			List<CartItemModel> Cart = HttpContext.Session.Getjson<List<CartItemModel>>("Cart");
			CartItemModel cartitem = Cart.Where(x => x.ProductID == id).FirstOrDefault();
			if (cartitem.Quantati > 1)
			{
				--cartitem.Quantati;
				HttpContext.Session.Setjson("Cart", Cart);
				return Ok(new { success = true, quantity = cartitem.Quantati, productId = id });
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

			return Ok(new { success = true, quantity = 0, productId = id });
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