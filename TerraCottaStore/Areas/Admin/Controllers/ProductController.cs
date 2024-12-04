using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin,Author")]
    public class ProductController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController (DataContext datacontex,IWebHostEnvironment webHostEnvironment)
        {
            _datacontext = datacontex;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _datacontext.Products.OrderByDescending(p => p.Id).Include(p => p.Brand).Include(p=> p.Category).ToListAsync());
        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name",product.CategoryID);
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name",product.BrandID);
            if ( ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                var Slug =await _datacontext.Products.FirstOrDefaultAsync(x=> x.Slug==product.Slug);
                if (Slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong hệ thống");
                    return View(product);
                }
               
                    if (product.imageupload!=null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath,"media/products");
                        string imagename = Guid.NewGuid().ToString() + "_" +product.imageupload.FileName;
                        string filepath = Path.Combine(uploadsDir, imagename);

                        FileStream fs = new FileStream(filepath, FileMode.Create);
                        await product.imageupload.CopyToAsync(fs);
                        fs.Close();
                        product.image=imagename;
                    }
                product.status = 1; 
                _datacontext.AddAsync(product);
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
            
            
            return View(product);
        }
        public async Task<IActionResult> Edit (int id)
        {   ProductModel product = await _datacontext.Products.FindAsync(id);
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name", product.CategoryID);
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name", product.BrandID);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ProductModel product)
        {
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name", product.CategoryID);
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name", product.BrandID);
            var exits_product = _datacontext.Products.Find(id);
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                var Slug = await _datacontext.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (Slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong hệ thống");
                    return View(product);
                }

                if (product.imageupload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imagename = Guid.NewGuid().ToString() + "_" + product.imageupload.FileName;
                    string filepath = Path.Combine(uploadsDir, imagename);
                    string oldfileimage = Path.Combine(uploadsDir, exits_product.image);
                    try
                    {
                        if (System.IO.File.Exists(oldfileimage))
                        {
                            System.IO.File.Delete(oldfileimage);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Error when delete image");
                    }
                    FileStream fs = new FileStream(filepath, FileMode.Create);
                    await product.imageupload.CopyToAsync(fs);
                    fs.Close();
                    exits_product.image = imagename;
                    
                }
                exits_product.Name = product.Name;
                exits_product.Price = product.Price;
                exits_product.Description = product.Description;
                exits_product.CategoryID = product.CategoryID;
                exits_product.BrandID = product.BrandID;
                exits_product.status = 1;
                _datacontext.Update(exits_product);
                await _datacontext.SaveChangesAsync();

                TempData["success"] = "update successfully!";
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


            return View(product);
        }
    
        public async Task<IActionResult> Shutdown(int id)
        {
            ProductModel product = await _datacontext.Products.FindAsync(id);
            product.status = 0;
            _datacontext.Update(product);
            await _datacontext.SaveChangesAsync();
            TempData["error"] = "Product shut down!";
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Onlive(int id)
        {
            ProductModel product = await _datacontext.Products.FindAsync(id);
            product.status = 1;
            _datacontext.Update(product);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "Product turn on!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> delete(int id)
        {
            ProductModel product = await _datacontext.Products.FindAsync(id);
           
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/produts");
                string oldfileimage = Path.Combine(uploadDir, product.image);
            try
            {
                if (System.IO.File.Exists(oldfileimage))
                {
                    System.IO.File.Delete(oldfileimage);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error when delete image");
            }
            _datacontext.Products.Remove(product);
            await _datacontext.SaveChangesAsync();
            TempData["error"] = "Delete suceesful!";

            return RedirectToAction("Index");   
        }   
    }

}
