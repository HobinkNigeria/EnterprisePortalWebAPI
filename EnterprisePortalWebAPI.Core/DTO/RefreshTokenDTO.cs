﻿namespace EnterprisePortalWebAPI.Core.DTO
{
	public class RefreshTokenDTO
	{
		public string Token { get; set; } = string.Empty;

	}
	public class RefreshTokenReqDTO
	{
		public string Token { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
	}
}
