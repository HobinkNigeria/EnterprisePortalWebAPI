using EnterprisePortalWebAPI.Core.DTO;
namespace EnterprisePortalWebAPI.Service.Interface
{
	public interface IOneTimePasswordService 
	{
		Task<Responses> GenerateOTP(OneTimePasswordDTO request);
		Task<Responses> ValidateOTP(ValidateOneTimePasswordDTO request);
	}
}
