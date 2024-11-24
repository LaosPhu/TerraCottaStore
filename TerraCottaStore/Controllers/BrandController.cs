
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContext _datacontext;

		public BrandController(DataContext dataContext) 
		{
		 _datacontext = dataContext;
		}

		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brand = _datacontext.Brands.Where(c => c.slug == Slug).FirstOrDefault();
			if (brand == null)
			{
				return RedirectToAction("Index");
			}

			var productByBrand = _datacontext.Products.Where(c => c.CategoryID == brand.Id);

			return View(await productByBrand.OrderByDescending(C => C.Id).ToListAsync());
		}
	}
}
