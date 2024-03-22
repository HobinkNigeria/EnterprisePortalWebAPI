using EnterprisePortalWebAPI.Core.Enum;
namespace EnterprisePortalWebAPI.Core.Domain
{
	public class OneTimePassword : BaseModel
	{
		public required string Email { get; set; }
		public required string OTP { get; set; }
		public DateTime ExpiryTime { get; set; }
		public bool IsUsed { get; set; }
		public OneTimePasswordPurpose Purpose { get; set; }
	}
}
