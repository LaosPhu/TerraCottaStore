using Microsoft.AspNetCore.Mvc;

namespace TerraCottaStore.Controllers
{
	public class CategoryController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}	
	}
}
