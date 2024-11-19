using System.ComponentModel.DataAnnotations;

namespace TerraCottaStore.Models
{
	public class CategoryModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4,ErrorMessage ="Yêu cầu nhập tên Danh Mục")]
		public string Name { get; set; }
		[Required, MinLength(4,ErrorMessage ="Nhập mô tả danh mục")]
		public string Description { get; set; }
		[Required]
		public int Slug { get; set; }
		public int status { get; set; }
	}
}
