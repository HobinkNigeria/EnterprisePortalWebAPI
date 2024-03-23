using EnterprisePortalWebAPI.Core.DTO;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EnterprisePortalWebAPI.Utility.Services
{
    public class JwtService(IConfiguration config, IActionContextAccessor accessor) : IJwtService
    {
        private readonly string _jwtSecret = config.GetSection("JWTSettings").GetSection("Secret").Value!;
        private readonly string _jwtIssuer = config.GetSection("JWTSettings").GetSection("Issuer").Value!;
        private readonly string _expDate = config.GetSection("JWTSettings").GetSection("ExpirationInMinutes").Value!;
        private readonly IActionContextAccessor _accessor = accessor;

        public async Task<GenerateTokenDTO> GenerateToken(string email)
        {
            string token = await GenerateToken(email, GetUserIpAddress());
            if (!string.IsNullOrWhiteSpace(token))
            {
                return new GenerateTokenDTO(email, token, string.Empty);
            }
            else
                return new GenerateTokenDTO(email, string.Empty, string.Empty);
        }

        public static RefreshTokenDTO GenerateRefreshToken()
        {
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            var randomBytes = new byte[64];
            randomNumberGenerator.GetBytes(randomBytes);
            return new RefreshTokenDTO
            {
                Token = Convert.ToBase64String(randomBytes)
            };
        }
        private static string GetSessionUser(string AuthToken)
        {
            var handler = new JwtSecurityTokenHandler();
            AuthToken = AuthToken.Replace("Bearer ", string.Empty);
            var jsonToken = handler.ReadToken(AuthToken);
            var token = handler.ReadToken(AuthToken) as JwtSecurityToken;
            return token?.Claims.First(claim => claim.Type == "Email").Value!;

        }
        public async Task<Responses> RefreshToken(RefreshTokenReqDTO request)
        {
            var response = new Responses(false);
            try
            {
                var ip = GetUserIpAddress();
                var email = GetSessionUser(request.Token);
                if (email == null || !email.IEquals(request.Email))
                {
                    response.Error = new ErrorResponse()
                    {
                        ResponseCode = "99",
                        ResponseDescription = "Invalid token provided"
                    };
                    response.IsSuccessful = false;
                    return response;
                }
                var tokenGenerated = string.Empty;


                tokenGenerated = await GenerateToken(request.Email, ip);
                DateTime expires = DateTime.Now.AddMinutes(double.Parse(_expDate));

                var res = new GenerateTokenDTO(request.Email, tokenGenerated, GenerateRefreshToken().Token);
                response.IsSuccessful = true;
                response.Data = res;
                return response;
            }
            catch (Exception)
            {
                response.Data = new GenerateTokenDTO(request.Email, string.Empty, string.Empty);
                response.IsSuccessful = false;
                return response;
            }
        }
        private string GetUserIpAddress()
        {
            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress;
            return ip.ToString();
        }
        private async Task<string> GenerateToken(string email, string ip)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                                new Claim("Email", email),
                                new Claim("Ip", ip),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                         };

            var token = new JwtSecurityToken(_jwtIssuer,
                    _jwtIssuer,
                    claims,
                    expires: DateTime.Now.AddMinutes(double.Parse(_expDate)),
                    signingCredentials: credentials);

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

    }
}
