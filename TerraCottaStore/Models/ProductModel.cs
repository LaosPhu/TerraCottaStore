using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TerraCottaStore.Repository.Validation;

namespace TerraCottaStore.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		[Required( ErrorMessage = "Yêu cầu nhập tên Sản Phẩm")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả Sản Phẩm")]
		public string Description { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập giá Sản Phẩm")]
		[Range(0.01,double.MaxValue)]
		[Column(TypeName = "decimal(8, 2)" )]
		public decimal Price { get; set; }
		[Required, Range (1,int.MaxValue,ErrorMessage ="Thêm 1 thương hiệu")]
		public int BrandID {  get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Thêm 1 Danh mục")]
        public int CategoryID { get; set; }
		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
		public string Slug {  get; set; }
		public string image { get; set; } = "noimage.jpg";
		public int status { get; set; }
		[NotMapped]
		[FileExtension]
		public IFormFile imageupload { get; set; }
	}
}
