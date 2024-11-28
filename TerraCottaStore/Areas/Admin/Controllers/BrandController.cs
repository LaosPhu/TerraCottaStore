using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BrandController : Controller
    {
        private readonly DataContext _datacontext;

        public BrandController(DataContext dataContext ) 
        {
         _datacontext = dataContext;
        }   

        public async Task <IActionResult> Index()
        {
            return View(await _datacontext.Brands.OrderByDescending(p => p.Id).ToListAsync());
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {

            if (ModelState.IsValid)
            {
                brand.slug = brand.Name.Replace(" ", "-");
                var Slug = await _datacontext.Brands.FirstOrDefaultAsync(x => x.slug == brand.slug);
                if (Slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong hệ thống");
                    return View(brand);
                }


                _datacontext.AddAsync(brand);
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


            return View(brand);
        }
        public async Task<IActionResult> delete(int id)
        {
            BrandModel brand = await _datacontext.Brands.FindAsync(id);


            _datacontext.Brands.Remove(brand);
            await _datacontext.SaveChangesAsync();
            TempData["error"] = "Delete suceesful!";

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            BrandModel brand = await _datacontext.Brands.FindAsync(id);
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {

            if (ModelState.IsValid)
            {
                brand.slug = brand.Name.Replace(" ", "-");
                var Slug = await _datacontext.Brands.FirstOrDefaultAsync(x => x.slug == brand.slug);
                if (Slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong hệ thống");
                    return View(brand);
                }


                _datacontext.Update(brand);
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


            return View(brand);
        }
        public async Task<IActionResult> Shutdown(int id)
        {
            BrandModel brand = await _datacontext.Brands.FindAsync(id);


            brand.status = 0;
            _datacontext.Brands.Update(brand);
            await _datacontext.SaveChangesAsync();
            TempData["error"] = "Setoff suceesful!";

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Turnon(int id)
        {
            BrandModel brand = await _datacontext.Brands.FindAsync(id);


            brand.status = 1;
            _datacontext.Brands.Update(brand);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "turn on suceesful!";

            return RedirectToAction("Index");
        }
    }
}
