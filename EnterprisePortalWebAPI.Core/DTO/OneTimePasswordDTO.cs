using EnterprisePortalWebAPI.Core.Enum;
namespace EnterprisePortalWebAPI.Core.DTO
{
	public class OneTimePasswordDTO
	{
		public required string Email { get; set; }
		public OneTimePasswordPurpose Purpose { get; set; }
	}
	public class ValidateOneTimePasswordDTO
	{
		public required string Email { get; set; }
		public required string OTP { get; set; }
		public OneTimePasswordPurpose Purpose { get; set; }
	}
}
