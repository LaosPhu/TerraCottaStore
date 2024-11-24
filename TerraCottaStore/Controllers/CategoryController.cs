using Microsoft.AspNetCore.Mvc;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace TerraCottaStore.Controllers
{
	public class CategoryController : Controller
	{
		private readonly DataContext _datacontext;

		public CategoryController(DataContext dataContext)
		{
		 _datacontext = dataContext;
		}
		public async Task <IActionResult> Index(String Slug ="")
		{	CategoryModel category = _datacontext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
			if (category == null)
			{
				return RedirectToAction("Index");
			}
				
			var productByCategory = _datacontext.Products.Where(c => c.CategoryID == category.Id);

			return View(await productByCategory.OrderByDescending(C => C.Id).ToListAsync());
		}	
	}
}
