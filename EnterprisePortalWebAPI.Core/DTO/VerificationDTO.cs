using EnterprisePortalWebAPI.Core.Enum;

namespace EnterprisePortalWebAPI.Core.DTO
{
	public class VerificationDTO
	{
		public bool IsVerified { get; set; }
		public VerificationStage VerificationStage { get; set; }
		public string VerificationRemark { get; set; } = string.Empty;
		public string VerificationOfficer { get; set; } = string.Empty;
	}
}
