using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Author")]
	public class RoleController : Controller
	{
		private readonly RoleManager<IdentityRole> _rolemanger;
		public RoleController(RoleManager<IdentityRole> rolemanager)
		{
			_rolemanger = rolemanager;
		}


		public async Task<IActionResult> Index()
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
		public async Task<IActionResult> Create(IdentityRole role)
		{

			if (ModelState.IsValid)
			{
				var Role = await _rolemanger.Roles.FirstOrDefaultAsync(x => x.Name == role.Name);
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
		public async Task<IActionResult> delete(string Id)
		{ var role = await _rolemanger.FindByIdAsync(Id);

            var result =await _rolemanger.DeleteAsync(role);
			if (result.Succeeded)
			{
                TempData["success"] = "Delete successfully!";
              
            }
			else
			{
                TempData["error"] = "Delete fail!";
               
            }
            return RedirectToAction("Index");

        }
		public async Task <IActionResult> Edit (string id)
		{
            var role = await _rolemanger.FindByIdAsync(id);
            return View(role);
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,IdentityRole role)
        {
			if (ModelState.IsValid)
			{
				var newrole = await _rolemanger.FindByNameAsync(role.Name);
				if (newrole != null)
                {
                    ModelState.AddModelError("", "role đã có trong hệ thống");
                    return View(role);
                }
                await  _rolemanger.UpdateAsync(role);
			
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

            return View(role);
        }
    }
}
