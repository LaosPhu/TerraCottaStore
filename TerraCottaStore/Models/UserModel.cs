using System.ComponentModel.DataAnnotations;

namespace TerraCottaStore.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required (ErrorMessage ="Nhập tên đăng nhập")]
		public string Userame { get; set; }
		[Required(ErrorMessage = "Nhập Gmail")]
		public string Email { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage ="Nhập PASSWORD")]

		public string Password { get; set; }

	}
}
