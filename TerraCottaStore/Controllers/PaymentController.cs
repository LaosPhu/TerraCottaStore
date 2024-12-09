using Microsoft.AspNetCore.Mvc;
using TerraCottaStore.Models.VNaymodel;
using TerraCottaStore.Services.VNPay;
namespace TerraCottaStore.Controllers

{
	public class PaymentController : Controller
	{
		private readonly IVnPayService _vnPayService;
		public PaymentController(IVnPayService vnPayService)
		{

			_vnPayService = vnPayService;
		}
		[HttpPost]
		public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
		{
			var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

			return Redirect(url);
		}
		
	}
}
