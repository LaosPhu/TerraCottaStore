using TerraCottaStore.Models.VNaymodel;

namespace TerraCottaStore.Services.VNPay
{
	public interface IVnPayService
	{
		string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
		PaymentResponseModel PaymentExecute(IQueryCollection collections);

	}
}
