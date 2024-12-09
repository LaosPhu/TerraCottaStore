using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _datacontext;

        public UserController (DataContext dataContext)
        {
            _datacontext = dataContext;
        }
        public async Task<IActionResult> Index()
        {


            return View(await _datacontext.Orders.OrderByDescending(p => p.UserName==User.Identity.Name).ToListAsync());

        }
        public async Task<IActionResult> ViewOrderDetail (string ordercode)
        {
            var detailorder = await _datacontext.OrderDetails.Include(dt => dt.Product).Where(d => d.OrderCode == ordercode).ToListAsync();

            return View(detailorder);
        }
    }
}
