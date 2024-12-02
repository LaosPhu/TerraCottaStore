using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _rolemanger;
        public RoleController(RoleManager<IdentityRole> rolemanager)
        {
            _rolemanger = rolemanager;
        }


        public async Task<IActionResult> Index ()
        {
            var role = await _rolemanger.Roles.ToListAsync();
            return View(role);
        }
        public IActionResult Create()
        {   

            return View();
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task <IActionResult> Create (IdentityRole role)
        {
		
			if (ModelState.IsValid)
			{
				var Role = await _rolemanger.Roles.FirstOrDefaultAsync(x=> x.Name == role.Name);
				if (Role != null)
				{
					ModelState.AddModelError("", "Danh mục đã có trong hệ thống");
                    return RedirectToAction("Index");
                }


				await _rolemanger.CreateAsync(role);
				
				TempData["success"] = "add successfully!";
				return RedirectToAction("Index");

			}
			else
			{
				TempData["error"] = "Got some error in role    !";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				string errormessage = string.Join("\n", errors);
				return BadRequest(errormessage);
			}


			return View("Create");
		}
    }

}
