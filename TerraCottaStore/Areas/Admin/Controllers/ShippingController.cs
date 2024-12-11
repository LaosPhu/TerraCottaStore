using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Author")]
	[Route("Admin/Shipping")]
	public class ShippingController : Controller
    {
        private readonly DataContext _datacontext;

        public ShippingController(DataContext dataContext) 
        {
         _datacontext = dataContext;
        }
        [Route("Index")]   
        public async Task< IActionResult> Index()
        {
            var shippingList = await _datacontext.Shippings.ToListAsync();
            ViewBag.ShippingList = shippingList;
            return View();
        }
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id )
        {
            try
            {
                // Xử lý xóa dữ liệu từ database
                var entity = await _datacontext.Shippings.FindAsync(id);
                if (entity != null)
                {
                    _datacontext.Shippings.Remove(entity);
                     await _datacontext.SaveChangesAsync();

                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Không tìm thấy dữ liệu." });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
        
        [HttpPost]
        [Route("CreatShipping")]
        public async Task<IActionResult> CreatShipping(ShippingModel shipping,string phuong,string tinh,string quan, decimal price)
        {
            shipping.city = tinh;
            shipping.ward = phuong;
            shipping.Distric = quan;
            shipping.Price = price;
            try
            {
                var existShiping = await _datacontext.Shippings.AnyAsync(s => s.city == tinh && s.Distric == quan && s.ward == phuong);

                if (existShiping)
                {
                    return Ok(new { duplicate = true, message = "Dữ liệu trùng lập " });
                }
                _datacontext.Shippings.Add(shipping);
                await _datacontext.SaveChangesAsync();
                return Ok(new { success = true, message = "Them dư liệu thành công " });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, "an error has occurred");
            }
            return View(shipping);
        }
    }
}
