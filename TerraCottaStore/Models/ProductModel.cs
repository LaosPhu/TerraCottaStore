using System.ComponentModel.DataAnnotations;

namespace TerraCottaStore.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên Sản Phẩm")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả Sản Phẩm")]
		public string Description { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập giá Sản Phẩm")]
		public decimal Price { get; set; }

		public int BrandID {  get; set; }
		public int CategoryID { get; set; }
		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
		public string Slug {  get; set; }
		public string image { get; set; }
		public int status { get; set; }
	}
}
