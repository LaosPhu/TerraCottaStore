using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Author")]
    
    public class OrderController : Controller
	{
        private readonly DataContext _datacontext;

        public OrderController (DataContext dataContext) 
		{
			_datacontext = dataContext;
		 
		}
		public async Task<IActionResult> Index()
		{
            

            return View(await _datacontext.Orders.OrderByDescending(p => p.Id).ToListAsync());
            
		}
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var detailorder = await _datacontext.OrderDetails.Include(dt => dt.Product).Where(d=>d.OrderCode==ordercode).ToListAsync();

            return View(detailorder);
        }
        [HttpPost]
       
        public async Task<IActionResult> UpdateOrder(int status,string ordercode)
        {
          var order = await _datacontext.Orders.FirstOrDefaultAsync(o => o.OrderCode==ordercode);
            if (order == null)
            {
                return  NotFound();

            }
            order.Status = status;
            try
            {
                await _datacontext.SaveChangesAsync();
                return Ok(new { success = true, message = "Order update successfully" });
            }
            catch (Exception ex)
            { 
                return StatusCode(500,"An error has occur");
            }
        }
       
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _datacontext.Orders.FindAsync(id);
            var list = await _datacontext.OrderDetails.Where(od => od.OrderCode==order.OrderCode).ToListAsync();
          
            if (list != null)
            {
                foreach (var item in list)
                { var productmodul = await _datacontext.Products.Where(pro => pro.Id==item.ProductId).FirstOrDefaultAsync();
                    productmodul.Quantity += item.Quantati;
                    _datacontext.Update(productmodul);
                    _datacontext.Remove(item);
                    
                }
              
               _datacontext.Remove(order);
                await _datacontext.SaveChangesAsync();
            }
            return RedirectToAction("Index");   
        }
    }
}
