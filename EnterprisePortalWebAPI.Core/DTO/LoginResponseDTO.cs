using EnterprisePortalWebAPI.Core.Enum;

namespace EnterprisePortalWebAPI.Core.DTO
{
	public class LoginResponseDTO
	{
		public required string Token { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public string CooperateID { get; set; } = string.Empty;
		public required string Email { get; set; }
		public UserRole Role { get; set; }
		public DateTime LastLogin { get; set; }
		public List<BuinessAvailable>? Businesses { get; set; }
	}
	public class BuinessAvailable
	{
		public required string BusinessId { get; set; }
		public required string BusinessName { get; set; }
	}
}
