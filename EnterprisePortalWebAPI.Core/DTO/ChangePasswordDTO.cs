namespace EnterprisePortalWebAPI.Core.DTO
{
	public class ChangePasswordDTO
	{
		public required string Email { get; set; }
		public required string OldPassword { get; set; }
		public required string NewPassword { get; set; }
	}
	public class ForgetPasswordDTO
	{
		public required string Email { get; set; }
	}
}
