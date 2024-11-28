using System.ComponentModel.DataAnnotations;

namespace TerraCottaStore.Models.ViewModel
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Nhập tên đăng nhập")]
		public string Userame { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage = "Nhập Password")]

		public string Password { get; set; }
		public string returnURL {get; set;}
	}
}
