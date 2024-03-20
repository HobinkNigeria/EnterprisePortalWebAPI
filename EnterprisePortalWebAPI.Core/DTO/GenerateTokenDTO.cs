using System;

namespace EnterprisePortalWebAPI.Core.DTO
{
	public class GenerateTokenDTO(string email, string jwtToken, string refreshToken)
	{
		public string Email { get; set; } = email;
		public string JwtToken { get; set; } = jwtToken;
		public string RefreshToken { get; set; } = refreshToken;
	}
}
