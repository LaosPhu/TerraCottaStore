using Microsoft.AspNetCore.Mvc;

namespace TerraCottaStore.Controllers
{
	public class CartController : Controller
	{
		public IActionResult index()
		{
			return View();
		}
		public IActionResult Checkout()
		{
			return View("~/Views/Checkout/Index.cshtml");
		}
	}
}