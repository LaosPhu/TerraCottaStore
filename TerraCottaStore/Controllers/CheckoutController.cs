using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TerraCottaStore.Areas.Admin.Repository;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;
using TerraCottaStore.Services.VNPay;

namespace TerraCottaStore.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _datacontext;
        private readonly IEmailSender _emailSender;
		private readonly IVnPayService _vnPayService;
		public CheckoutController(IEmailSender emailSender,DataContext dataContext, IVnPayService vnPayService)
		{
			_vnPayService = vnPayService;
			_datacontext = dataContext;
			 _emailSender = emailSender;

        }
		public ActionResult Index()
		{
            List<CartItemModel> Items = HttpContext.Session.Getjson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemViewModel CartVM = new()
            {
                CartItems = Items,
                GrandTotal = Items.Sum(a => a.Quantati * a.Price),

            };
            return View(CartVM);
        }

		[HttpPost]
		
		public async Task<IActionResult> Checkout (string PaymentMethob, string paymentId)
		{
			
			var userEmail =User.FindFirstValue(ClaimTypes.Email);
			if (string.IsNullOrWhiteSpace(userEmail))
			{ return RedirectToAction("Login", "Account"); }
			else
			{
				var ordercode =Guid.NewGuid().ToString();
				var orderitem = new OrderModel();
				orderitem.OrderCode = ordercode;
				if (PaymentMethob == null)
				{
					orderitem.OrderMethob = "COD";
				}
				else
				{
					if (PaymentMethob == "VnPay")
					{
						orderitem.OrderMethob ="VnPay"+paymentId;
					}
					else
					{
						if (PaymentMethob == "Momo")
							orderitem.OrderMethob = "Momo"+PaymentMethob;
					}
				}
				orderitem.UserName = userEmail;
				orderitem.Status = 1;
				orderitem.CreatedDate = DateTime.Now;
				_datacontext.Add(orderitem);
				await _datacontext.SaveChangesAsync();
				List <CartItemModel> cartItems = HttpContext.Session.Getjson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach (var cart in cartItems)
				{
					var orederdetail = new OrderDetails();
					orederdetail.UserName =userEmail;
					orederdetail.OrderCode =ordercode;
					orederdetail.ProductId = cart.ProductID;
					orederdetail.Quantati  = cart.Quantati;
					orederdetail.Price     = cart.Price;
					_datacontext.Add(orederdetail);
					_datacontext.SaveChanges();
				}

                foreach (var cart in cartItems)
                { var productmodle = await _datacontext.Products.FirstOrDefaultAsync(p=> p.Id == cart.ProductID);
					productmodle.Quantity -= cart.Quantati;
					if (productmodle.Quantity==0) productmodle.status = 0;
					_datacontext.Update(productmodle);
					await _datacontext.SaveChangesAsync();
                }
              
				TempData["success"] = "Tao don hang thanh cong";
				emailpos(userEmail);



                HttpContext.Session.Remove("Cart");
                return RedirectToAction("Index","Cart");
			}
			return View();
		}
		public async void emailpos (string email)
		{	
            var receiver = email;
            var subject = "Thinh dep trai";
            var Message = "test sever";
            await _emailSender.SendEmailAsync(receiver, subject, Message);
        }
		[HttpGet]
		public async Task <IActionResult> PaymentCallbackVnpay()
		{
			var response = _vnPayService.PaymentExecute(Request.Query);
			if (response.VnPayResponseCode == "00")
			{
				var VnpayInsert = new VnPayModel
				{
					OrderId = response.OrderId,
					PaymentMethod = response.PaymentMethod,
					OrderDescription = response.OrderDescription,
					TransactionId = response.TransactionId,
					PaymentId = response.PaymentId,
					DateCreated = DateTime.Now,
				};
				_datacontext.Add(VnpayInsert);
				await _datacontext.SaveChangesAsync();

				var PaymentMethob = response.PaymentMethod;
				var PaymentId	= response.PaymentId;
				await Checkout(PaymentMethob,PaymentId);
				
			}
			else
			{
				return RedirectToAction("ErrorVnPayment","Checkout", response.VnPayResponseCode);
			}
			return View(response);

		}

		public IActionResult ErrorVnPayment (string error)
		{
            switch (error)
            {
                case "01":
                    error = "Giao dịch đã tồn tại";
                    break;

                case "02":
                    error = "Merchant không hợp lệ (kiểm tra lại vnp_tmn_code)";
                    break;

                case "04":
                    error = "Khởi tạo GD không thành công do Website đang bị tạm khóa";
                    break;

                case "08":
                    error = "Giao dịch không thành công do: Hệ thống Ngân hàng đang bảo trì. Xin quý khách tạm thời không thực hiện giao dịch bằng thẻ/tài khoản của Ngân hàng này";
                    break;

                case "24":
                    error = "Giao dịch bị hủy";
                    break;

                case "79":
                    error = "Khác hàng thực hiện xác thực sai quá số lần cho phépy";
                    break;
                case "97":
                    error = "Sai chữ ký";
                    break;
                default:
                    error = "Lỗi không xác đinh khi giao dịch vnpay";
                    break;
            }

            return View(model :error);
		}

    }
}
