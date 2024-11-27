using Microsoft.AspNetCore.Mvc;

namespace TerraCottaStore.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
