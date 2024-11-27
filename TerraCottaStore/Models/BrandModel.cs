using System.ComponentModel.DataAnnotations;

namespace TerraCottaStore.Models
{
	public class BrandModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên Brand")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả Brand")]
		public string Description { get; set; }
		
		public string slug { get; set; }
		public int status { get; set; }	

	}
}
