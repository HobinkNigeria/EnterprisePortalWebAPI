using EnterprisePortalWebAPI.Core.DTO;

namespace EnterprisePortalWebAPI.Service.Interface
{
	public interface IMenuService
	{
		Task<Responses> Create(MenuDTO request);
		Task<Responses> Update(MenuDTO request, string menuId);
		Task<Responses> Delete(string menuId);
		Task<Responses> Get(string menuId);
		Responses GetMenu(ClientParameters parameters, string cooperateId);
	}
}
