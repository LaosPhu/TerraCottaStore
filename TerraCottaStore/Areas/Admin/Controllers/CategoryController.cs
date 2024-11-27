using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly DataContext _datacontext;

		public CategoryController(DataContext dataContext)
		{
			_datacontext = dataContext;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _datacontext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}
	}
}
