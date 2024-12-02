using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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
    }
}
