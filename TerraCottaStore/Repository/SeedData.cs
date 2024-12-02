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
                        new ProductModel { Name = "Bình Gốm", Slug = "sp-binh-gom", Description = "Bình gốm Minh Long", image = "1.jpg", Brand = Minh_Long, Category = Ceramic_Pot, Price = 500000, status = 1 },
                        new ProductModel { Name = "Chén Cổ", Slug = "sp-chen-co", Description = "Chén cổ Bát Tràng", image = "2.jpg", Brand = Bat_Trang, Category = Dining_room, Price = 240000, status = 1 }
                    );
                    _context.SaveChanges();
                }

          
		}
	}
}
