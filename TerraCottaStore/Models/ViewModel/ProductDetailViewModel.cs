using System.ComponentModel.DataAnnotations;

namespace TerraCottaStore.Models.ViewModel
{
	public class ProductDetailViewModel
	{
		public ProductModel ProductDetail { get; set; }
		[Required(ErrorMessage ="Yêu cầu nhập đánh giá sản phẩm")]
		public string Comment {  get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập tên")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập email ")]
		public string Email { get; set; }
	}
}
