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
            ViewBag.Category = new SelectList(_datacontext.Categories, "Id", "Name");
            ViewBag.Brand = new SelectList(_datacontext.Brands, "Id", "Name");
            if( ModelState.IsValid)
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
                        string imagename = Guid.NewGuid().ToString() +"_"+product.imageupload.FileName;
                        string filepath = Path.Combine(uploadsDir, imagename);
                        FileStream fs = new FileStream(filepath, FileMode.Create);
                        await product.imageupload.CopyToAsync(fs);
                        fs.Close();
                        product.image=imagename;
                    }
                
                _datacontext.Add(product);
                _datacontext.SaveChangesAsync();
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
    }
}
