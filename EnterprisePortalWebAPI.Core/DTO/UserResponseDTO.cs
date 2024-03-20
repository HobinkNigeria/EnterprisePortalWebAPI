using EnterprisePortalWebAPI.Core.Enum;
namespace EnterprisePortalWebAPI.Core.DTO
{
	public class UserResponseDTO
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required string CooperateID { get; set; }
		public required string Email { get; set; }
		public DateTime PasswordLastChanged { get; set; }
		public DateTime LastLogin { get; set; }
		public UserRole Role { get; set; }
		public bool IsActive { get; set; }
		public required string Id { get; set; } 
		public DateTime DateCreated { get; set; }
		public DateTime DateUpdated { get; set; }
	}
}
