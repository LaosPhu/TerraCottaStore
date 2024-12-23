﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using TerraCottaStore.Models;

namespace TerraCottaStore.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
                _context.Database.Migrate();
                if (!_context.Products.Any())
                {
                    BrandModel Minh_Long = new BrandModel { Name = "Minh Long", slug = "minh-long", Description = "Công ty gốm sứ Minh Long", status = 1 };
                    BrandModel Bat_Trang = new BrandModel { Name = "Bát Tràng", slug = "Bat-Trang", Description = "Làng gốm sứ Bát Tràng", status = 1 };
                    CategoryModel Ceramic_Pot = new CategoryModel { Name = "Bình Gốm", Slug = "pot", Description = "Bình Gốm", status = 1 };
                    CategoryModel Dining_room = new CategoryModel { Name = "Bàn ăn", Slug = "Dining-room", Description = "Dụng cụ bàn ăn", status = 1 };

                    _context.Products.AddRange(
                        new ProductModel { Name = "Bình Gốm", Slug = "sp-binh-gom", Description = "Bình gốm Minh Long", image = "1.jpg",Quantity=10, Brand = Minh_Long, Category = Ceramic_Pot, Price = 500000, status = 1 },
                        new ProductModel { Name = "Chén Cổ", Slug = "sp-chen-co", Description = "Chén cổ Bát Tràng", image = "2.jpg", Quantity = 10, Brand = Bat_Trang, Category = Dining_room, Price = 240000, status = 1 }
                    );
                    _context.SaveChanges();
                }

          
		}
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string>
        {
            "Admin",
            "Manager",
            "User"
        };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedUsersAsync(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            var role = await roleManager.FindByNameAsync("Admin");
            
            var defaultUser = new AppUserModel
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true,
                RoleId = role.Id,
                
               
            };

            if (await userManager.FindByEmailAsync(defaultUser.Email) == null)
            {
                var createUserResult = await userManager.CreateAsync(defaultUser, "Password@123");
                if (createUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                }
            }
        }
    }
}
