using EnterprisePortalWebAPI.Core.Enum;

namespace EnterprisePortalWebAPI.Core.DTO
{
	public class UserDTO
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public string CooperateID { get; set; } = string.Empty;
		public required string Email { get; set; }
		public required string Password { get; set; }
		public UserRole Role { get; set; }
		public bool IsActive { get; set; } = true;
	}
}
