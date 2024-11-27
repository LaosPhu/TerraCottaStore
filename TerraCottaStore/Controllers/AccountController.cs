using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Controllers
{
	public class AccountController : Controller
	{
	
		private UserManager<AppUserModel> _usermange;
		private SignInManager<AppUserModel> _signInManager;
		public AccountController(UserManager<AppUserModel>	username, SignInManager<AppUserModel> signinmanage)
		{
		 _signInManager = signinmanage;
			_usermange = username;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task <IActionResult> Login()
		{
			return View();
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserModel user)
		{
			if (ModelState.IsValid)
			{
				AppUserModel newuser = new AppUserModel
				{
					UserName = user.Userame,
					Email = user.Email,

				};
				IdentityResult result = await _usermange.CreateAsync(newuser);
				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
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
