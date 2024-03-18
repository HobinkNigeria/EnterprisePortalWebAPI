namespace EnterprisePortalWebAPI.Core.DTO
{
	public class ChangePasswordDTO
	{
		public required string UserId { get; set; }
		public required string OldPassword { get; set; }
		public required string NewPassword { get; set; }
	}
}
