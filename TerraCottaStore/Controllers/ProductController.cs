using Microsoft.AspNetCore.Mvc;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _datacontext;

		public ProductController(DataContext dataContext) 
		{
		 _datacontext = dataContext;
		}
		public IActionResult index()
		{
			return View();
		}
		public async Task<IActionResult> ProductDetail(int Id )
		{	
			if (Id == null)
			{
				return RedirectToAction("Index");
			}
			var productdetail = _datacontext.Products.Where(c => c.Id == Id).FirstOrDefault();
			return View(productdetail);
		}

	}
}
