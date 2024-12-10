using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Repository;

namespace TerraCottaStore.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Author")]
	[Route("Admin/VnPay")]
	public class VnPayController : Controller
    {
        private readonly DataContext _datacontext;

        public VnPayController (DataContext datacontext)
        {
            _datacontext = datacontext;
        }
        [Route("Index")]
        public async Task <IActionResult> Index()
        {
           var data = await _datacontext.VnPayInfos.ToListAsync();
            return View(data);
        }
        [Route("DetailVnPay")]
        public async Task<IActionResult> DetailVnPay(string id)
        {
            var vnpayinfo = await _datacontext.VnPayInfos.Where(vnp => vnp.PaymentId == id).FirstOrDefaultAsync();
            return View(vnpayinfo);
        }
    }
}
