using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Models;
using TerraCottaStore.Models.ViewModel;
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
				var productdetail = await _datacontext.Products.Include(x=>x.Ratings).Where(c => c.Id == Id).FirstOrDefaultAsync();
				var relatedproduct = await _datacontext.Products.Where(p =>p.CategoryID == productdetail.CategoryID && productdetail.Id != p.Id).Take(5).ToListAsync();
				ViewBag.relatedproduct = relatedproduct;
				var relatedproductBrand = await _datacontext.Products.Where(p => p.BrandID == productdetail.BrandID && productdetail.Id != p.Id).Take(5).ToListAsync();
				ViewBag.relatedproductBrand = relatedproductBrand;
				var viewmodel = new ProductDetailViewModel 
				{
				 ProductDetail = productdetail,
				 
				};
				return View(viewmodel);
			}
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task <IActionResult> Rating(RatingModel rating)
		{
			if (ModelState.IsValid)
			{
				var newrating = new RatingModel
				{	
					ProductId = rating.ProductId,
					Name = rating.Name,
					Comment = rating.Comment,
					Email = rating.Email,
					Rating = rating.Rating,
					star  = rating.star,
				};
				await _datacontext.Ratings.AddAsync(newrating);
				await _datacontext.SaveChangesAsync();
			}
			else
			{
				TempData["error"] = "Got some error in model     !";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				string errormessage = string.Join("\n", errors);
				return RedirectToAction("ProductDetail",new { Id = rating.ProductId });
			}
			return RedirectToAction("ProductDetail", new { Id = rating.ProductId });
		}

		public async Task<IActionResult> Search (string searchTerm)
		{
			var product = await _datacontext.Products.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)).ToListAsync();
			ViewBag.Keyword = searchTerm;
			return View(product);
		}
	}
}
