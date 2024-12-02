using Microsoft.AspNetCore.Identity;

namespace TerraCottaStore.Models
{
	public class AppUserModel : IdentityUser
	{
		public string Occupation {  get; set; }
        public string RoleId { get; set; }
    }
}
