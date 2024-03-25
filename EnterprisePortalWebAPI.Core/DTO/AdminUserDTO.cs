using EnterprisePortalWebAPI.Core.Enum;

namespace EnterprisePortalWebAPI.Core.DTO
{
	public class AdminUserDTO
	{
		public required string OTP { get; set; }
		public OneTimePasswordPurpose Purpose { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required string Email { get; set; }
		public required string Password { get; set; }
		public bool IsActive { get; set; } = true;
	}
}
