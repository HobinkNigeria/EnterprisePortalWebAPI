namespace EnterprisePortalWebAPI.Core.DTO
{
	public class UpdateVerificationStatus
	{
		public required string BusinessId { get; set; }
		public string VerificationRemark { get; set; } = string.Empty;
	}
}
