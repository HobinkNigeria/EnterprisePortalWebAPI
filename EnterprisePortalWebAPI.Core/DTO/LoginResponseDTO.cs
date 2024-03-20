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
	}
}
