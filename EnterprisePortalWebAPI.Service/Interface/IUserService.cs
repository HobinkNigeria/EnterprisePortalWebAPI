using EnterprisePortalWebAPI.Core.DTO;

namespace EnterprisePortalWebAPI.Service.Interface
{
	public interface IUserService
	{
		Task<Responses> Create(UserDTO request, bool isAdditionalAccount);
		Task<Responses> CreateAdditionalAccount(UserDTO request);
		Task<Responses> Update(UserDTO request, string userId);
		Task<Responses> UpdatePassword(ChangePasswordDTO request);
		Task<Responses> Delete(string userId);
		Task<Responses> Get(string userId);
		Responses GetbyCooperate(ClientParameters parameters, string cooperateId);
		Task<Responses> Login(LoginDTO request);
		Task<Responses> RefreshLoginToken(RefreshTokenReqDTO request);
	}
}
