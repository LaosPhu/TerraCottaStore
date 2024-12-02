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
        [Required]
        [RegularExpression(@"^(\+84|0)[0-9]{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage ="Nhập PASSWORD")]

		public string Password { get; set; }

		public string RoleId { get; set; }
		//chinh role id thanh n var char max trong sql
	}
}
