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
    [Authorize(Roles = "Admin,Author")]
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
        public async Task<IActionResult> Index(int pg =1)
        {
            var list = await _appusermodel.Users.OrderByDescending(p => p.Id).ToListAsync();
            const int pageSize = 10; //10 items/trang

            if (pg < 1) //page < 1;
            {
                pg = 1; //page ==1
            }
            int recsCount = list.Count();

            var pager = new Paginate(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = list.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
          //  return View(await _appusermodel.Users.OrderByDescending(p=>p.Id).ToListAsync());
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
                    var createUser =await _appusermodel.FindByEmailAsync(user.Email);
                    var userId = createUser.Id;
                    var roleU = await _rolemanger.FindByIdAsync(user.RoleId);
                    var addrolerResult = await _appusermodel.AddToRoleAsync(createUser, roleU.Name);
                    if (!addrolerResult.Succeeded)
                    {
                        foreach (var error in creatUserResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                    }
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
        [HttpGet]

        [Route("Delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();

            }
            var user = await _appusermodel.FindByIdAsync(Id);
            if (user != null)
            {
                var deleteresult = await _appusermodel.DeleteAsync(user);
                if (!deleteresult.Succeeded)
                {
                    return View("Error");
                }
            }
            TempData["success"] = "Delete successfully!";
            return RedirectToAction("Index");
        }
        [HttpGet]

        [Route("Edit")]

        public async Task<IActionResult> Edit (string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();

            }
            var user = await _appusermodel.FindByIdAsync(Id);
            if (user == null)
            {   
                return NotFound();
            }
            var role = await _rolemanger.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(role, "Id", "Name");
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]

        public async Task<IActionResult> Edit(string Id,AppUserModel user)
        {   
            var exitinguser = await _appusermodel.FindByIdAsync(Id);
            if (exitinguser == null)
            {
                return NotFound();

            }
            if (ModelState.IsValid)
            {
                exitinguser.UserName = user.UserName;
                exitinguser.Email = user.Email;
                exitinguser.PhoneNumber = user.PhoneNumber;
                exitinguser.RoleId = user.RoleId;

            }
            var updateresult = await _appusermodel.UpdateAsync(exitinguser);
            if (updateresult.Succeeded)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                foreach (var errorr in updateresult.Errors)
                {
                    ModelState.AddModelError(string.Empty, errorr.Description);
                }
            }
            
            var role = await _rolemanger.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(role, "Id", "Name");

            TempData["error"] = "Model vadidation broke !";
            var error = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage)).ToList();
            string erromassage = string.Join("\n", error);
            return View(user);
        }


    }

}
