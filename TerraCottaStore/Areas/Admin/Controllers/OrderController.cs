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
		public async Task<IActionResult> Index(int pg=1)
		{
            var list = await _datacontext.Orders.OrderByDescending(p => p.Id).ToListAsync();
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
    }
}
