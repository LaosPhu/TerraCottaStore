using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TerraCottaStore.Models;
using TerraCottaStore.Models.ViewModel;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Controllers
{
	public class AccountController : Controller
	{
	
		private UserManager<AppUserModel> _usermange;
		private SignInManager<AppUserModel> _signInManager;
        private readonly RoleManager<IdentityRole> _rolemanger;
        public AccountController(UserManager<AppUserModel>	username, SignInManager<AppUserModel> signinmanage, RoleManager<IdentityRole> rolemanger)
		{
		 _signInManager = signinmanage;
			_usermange = username;
            _rolemanger = rolemanger;

        }
		
		public IActionResult Login(string returnURL)
		{
			return View(new LoginViewModel { returnURL=returnURL});
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Userame, loginVM.Password, false, false);
				if (result.Succeeded)
				{
					return Redirect(loginVM.returnURL ?? "/");
				}
				ModelState.AddModelError("", "Sự cố khi đăng nhập");
			}

			return View(loginVM);
		}
		public async Task <IActionResult> Logout (string returnURL)
		{
			await _signInManager.SignOutAsync();
			return Redirect(returnURL="/");
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserModel user)
		{  var role = await _rolemanger.FindByNameAsync("User");
            user.RoleId = role.Id;
            if (ModelState.IsValid)
			{
				AppUserModel newuser = new AppUserModel
				{
					UserName = user.Userame,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					RoleId = user.RoleId
				};

				IdentityResult result = await _usermange.CreateAsync(newuser, user.Password);
				if (result.Succeeded)
				{	
                    var addrolerResult = await _usermange.AddToRoleAsync(newuser, "User");
                    TempData["success"] = "Tao user thanh cong";
					return Redirect("/account/login");
				}
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View();
		}
	}
}
