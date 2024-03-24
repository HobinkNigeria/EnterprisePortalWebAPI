﻿using EnterprisePortalWebAPI.Core.Enum;

namespace EnterprisePortalWebAPI.Core.Domain
{
	public class User : BaseModel
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required string CooperateID { get; set; }
		public string BusinessID { get; set; } = string.Empty;
		public required string Email { get; set; }
		public required string Password { get; set; }
		public DateTime PasswordLastChanged { get; set; }
		public DateTime LastLogin { get; set; }
		public bool PasswordIsSystemGenerated { get; set; } = false;
		public UserRole Role { get; set; }
		public bool IsActive { get; set; } = true;
	}
}
