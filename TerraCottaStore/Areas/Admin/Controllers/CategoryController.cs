using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
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
        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var Slug = await _datacontext.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (Slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong hệ thống");
                    return View(category);
                }
                
               
                _datacontext.AddAsync(category);
                await _datacontext.SaveChangesAsync();

                TempData["success"] = "add successfully!";
                return RedirectToAction("Index");

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
                return BadRequest(errormessage);
            }


            return View(category);
        }
        public async Task<IActionResult> delete(int id)
        {
            CategoryModel category = await _datacontext.Categories.FindAsync(id);

           
            _datacontext.Categories.Remove(category);
            await _datacontext.SaveChangesAsync();
            TempData["error"] = "Delete suceesful!";

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {   CategoryModel category = await _datacontext.Categories.FindAsync(id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category)
        {

            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var Slug = await _datacontext.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (Slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong hệ thống");
                    return View(category);
                }


                _datacontext.Update(category);
                await _datacontext.SaveChangesAsync();

                TempData["success"] = "add successfully!";
                return RedirectToAction("Index");

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
                return BadRequest(errormessage);
            }


            return View(category);
        }
        public async Task<IActionResult> Shutdown(int id)
        {
            CategoryModel category = await _datacontext.Categories.FindAsync(id);


            category.status = 0;
            _datacontext.Categories.Update(category);
            await _datacontext.SaveChangesAsync();
            TempData["error"] = "Setoff suceesful!";

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Turnon(int id)
        {
            CategoryModel category = await _datacontext.Categories.FindAsync(id);


            category.status = 1;
            _datacontext.Categories.Update(category);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "turn on suceesful!";

            return RedirectToAction("Index");
        }
    }
}
