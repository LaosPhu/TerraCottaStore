using System.ComponentModel.DataAnnotations;

namespace TerraCottaStore.Models
{
	public class CategoryModel
	{
		[Key]
		public int Id { get; set; }
		[Required (ErrorMessage ="Yêu cầu nhập tên Danh Mục")]
		public string Name { get; set; }

		[Required (ErrorMessage ="Nhập mô tả danh mục")]
		public string Description { get; set; }
	
		public string Slug { get; set; }
		public int status { get; set; }
	}
}
