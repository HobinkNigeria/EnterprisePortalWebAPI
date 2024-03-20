using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EnterprisePortalWebAPI.Filters
{
	public static class AuthenticationExtension
	{
		public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration config)
		{
			var secret = config.GetSection("JWTSettings").GetSection("Secret").Value;

			var key = Encoding.ASCII.GetBytes(secret!);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.TokenValidationParameters = new TokenValidationParameters
				{

					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = config.GetValue<string>("JWTSettings:Issuer"),
					ValidAudience = config.GetValue<string>("JWTSettings:Issuer")
				};
			});

			return services;
		}
	}
}
