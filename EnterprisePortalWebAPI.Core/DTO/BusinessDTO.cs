using EnterprisePortalWebAPI.Core.Enum;

namespace EnterprisePortalWebAPI.Core.DTO
{
	public class BusinessDTO
	{
		public required string Name { get; set; }
		public required string CooperateID { get; set; }
		public required string Address { get; set; }
		public BusinessCategory BusinessCategory { get; set; }
		public string ResistrationNumber { get; set; } = string.Empty;
		public string ResistrationCertificateImageReference { get; set; } = string.Empty;
		public string NIN { get; set; } = string.Empty;
		public string IdentificationImageReference { get; set; } = string.Empty;
		public string PrimaryBrandColour { get; set; } = string.Empty;
		public string SecondaryBrandColour { get; set; } = string.Empty;
		public string LogoImageReference { get; set; } = string.Empty;
	}
}
