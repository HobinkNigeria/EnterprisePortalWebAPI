using EnterprisePortalWebAPI.Core.DTO;
namespace EnterprisePortalWebAPI.Service.Interface
{
	public interface IBusinessService
	{
		Task<Responses> Create(BusinessDTO request);
		Task<Responses> Update(BusinessDTO request, string businessId);
		Task<Responses> VerifyBusiness(VerificationDTO request, string businessId);
		Task<Responses> VerificationStageUpdate(UpdateVerificationStatus request);
		Task<Responses> Delete(string businessId);
		Task<Responses> Get(string businessId);
		Responses GetByCooperateId(ClientParameters parameters, string cooperateId);
	}
}
