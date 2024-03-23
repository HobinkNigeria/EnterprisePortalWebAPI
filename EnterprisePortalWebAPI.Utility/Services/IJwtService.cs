using EnterprisePortalWebAPI.Core.DTO;

namespace EnterprisePortalWebAPI.Utility.Services
{
    public interface IJwtService
    {
        Task<GenerateTokenDTO> GenerateToken(string email);
        Task<Responses> RefreshToken(RefreshTokenReqDTO request);

    }
}
