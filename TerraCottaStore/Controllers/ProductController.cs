using Microsoft.AspNetCore.Mvc;

namespace TerraCottaStore.Controllers
{
	public class ProductController : Controller
	{
		public IActionResult index()
		{
			return View();
		}
		public IActionResult ProductDetail()
		{
			return View();
		}

	}
}
