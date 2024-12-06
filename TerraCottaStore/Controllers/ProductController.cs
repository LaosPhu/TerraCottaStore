using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			/*if (Id == null)
			{
				return RedirectToAction("Index");
			}
			var productdetail = _datacontext.Products.Where(c => c.Id == Id).FirstOrDefault();
			return View(productdetail);
			*/
			{
				if (Id==null)
				{
					return RedirectToAction("Index");
				}
				var productdetail = _datacontext.Products.Where(c => c.Id == Id).FirstOrDefault();
				var relatedproduct = await _datacontext.Products.Where(p =>p.CategoryID == productdetail.CategoryID && productdetail.Id != p.Id).Take(5).ToListAsync();
				ViewBag.relatedproduct = relatedproduct;
				var relatedproductBrand = await _datacontext.Products.Where(p => p.BrandID == productdetail.BrandID && productdetail.Id != p.Id).Take(5).ToListAsync();
				ViewBag.relatedproductBrand = relatedproductBrand;
				return View(productdetail);
			}
		}

		public async Task<IActionResult> Search (string searchTerm)
		{
			var product = await _datacontext.Products.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)).ToListAsync();
			ViewBag.Keyword = searchTerm;
			return View(product);
		}
	}
}
