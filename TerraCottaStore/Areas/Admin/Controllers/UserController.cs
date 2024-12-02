using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
    [Route("Admin/User")]
	public class UserController : Controller
    {
        
        private readonly UserManager<AppUserModel> _appusermodel;
        private readonly RoleManager<IdentityRole> _rolemanger;

        public UserController(UserManager <AppUserModel> appUserModel,RoleManager<IdentityRole> roleManager) 
        {
        
        _appusermodel = appUserModel;
        _rolemanger = roleManager;
        }
        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _appusermodel.Users.OrderByDescending(p=>p.Id).ToListAsync());
        }

        [HttpGet]
        [Route("Create")]
      
        public async Task<IActionResult> Create()
        {   
            var role = await _rolemanger.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(role, "Id", "Name");
            return View(new AppUserModel());
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]

        public async Task<IActionResult> Create(AppUserModel user)
        {
            if (ModelState.IsValid)
            {
                var creatUserResult = await _appusermodel.CreateAsync(user,user.PasswordHash);
                if (creatUserResult.Succeeded)
                {
                    return RedirectToAction("Index","User");
                }else
                {
                    foreach (var error in creatUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
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
            var role = await _rolemanger.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(role, "Id", "Name");
            return View(user);
        }
        

    }

}
