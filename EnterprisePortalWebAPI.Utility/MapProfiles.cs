using AutoMapper;
using EnterprisePortalWebAPI.Core.Domain;
using EnterprisePortalWebAPI.Core.DTO;

namespace EnterprisePortalWebAPI.Utility
{
	public class MapProfiles : Profile
	{
		public MapProfiles()
		{
			CreateMap<MenuDTO, Menu>();
			CreateMap<Menu, MenuDTO>().ReverseMap();
			CreateMap<UserDTO,User>();
			CreateMap<User, LoginResponseDTO >().ReverseMap();
			CreateMap<User, UserDTO>().ReverseMap();
		}
	}
}
