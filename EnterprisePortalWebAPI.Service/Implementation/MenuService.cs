using AutoMapper;
using EnterprisePortalWebAPI.Core;
using EnterprisePortalWebAPI.Core.Domain;
using EnterprisePortalWebAPI.Core.DTO;
using EnterprisePortalWebAPI.Service.Interface;
using EnterprisePortalWebAPI.Utility;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EnterprisePortalWebAPI.Service.Implementation
{
	public class MenuService(DatabaseContext context, IMapper mapper) : IMenuService
	{
		private readonly DatabaseContext _context = context;
		private readonly IMapper _mapper = mapper;
		public async Task<Responses> Create(MenuDTO request)
		{
			var responses = new Responses(false);
			try
			{
				var menuItem = await _context.Menus.FirstOrDefaultAsync(x => x.Item.ToLower() == request.Item.ToLower() && x.CooperateID == request.CooperateID);

				if (menuItem is not null)
				{
					responses.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.MENU_ITEM_EXIST,
						ResponseDescription = $"Item {request.Item} has already been created"
					};
					responses.IsSuccessful = false;
					return responses;
				}

				var menuItemToCreate = _mapper.Map<Menu>(request);
				menuItemToCreate.DateCreated = DateTime.Now;
				menuItemToCreate.DateUpdated = DateTime.Now;

				_context.Add(menuItemToCreate);
				await _context.SaveChangesAsync();

				responses.IsSuccessful = true;
				responses.Data = menuItemToCreate;
				return responses;
			}
			catch (Exception ex)
			{
				responses.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = ex.Message
				};
				responses.IsSuccessful = false;
				return responses;
			}
		}

		public async Task<Responses> Update(MenuDTO request, string menuId)
		{
			var response = new Responses(false);
			try
			{
				var menuItem = await _context.Menus.FirstOrDefaultAsync(x => x.Id == menuId);
				if (menuItem is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"Menu item {request.Item} not found"
					};
					response.IsSuccessful = false;
					return response;
				}
				var menuItemToUpdate = _mapper.Map(request, menuItem);
				menuItemToUpdate.DateUpdated = DateTime.Now;

				_context.Update(menuItemToUpdate);
				await _context.SaveChangesAsync();

				response.IsSuccessful = true;
				response.Data = menuItemToUpdate;
				return response;
			}
			catch (Exception ex)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = ex.Message
				};
				response.IsSuccessful = false;
				return response;
			}
		}
		public async Task<Responses> Delete(string menuId)
		{
			var response = new Responses(false);
			try
			{
				var menuItem = await _context.Menus.FirstOrDefaultAsync(x => x.Id == menuId);
				if (menuItem is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"Menu item with reference{menuId} not found"
					};
					response.IsSuccessful = false;
					return response;
				}

				_context.Remove(menuItem);
				await _context.SaveChangesAsync();

				response.IsSuccessful = true;
				response.Data = menuItem;
				return response;
			}
			catch (Exception ex)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = ex.Message
				};
				response.IsSuccessful = false;
				return response;
			}
		}
		public async Task<Responses> Get(string menuId)
		{
			var response = new Responses(false);
			try
			{
				var menuItem = await _context.Menus.FirstOrDefaultAsync(x => x.Id == menuId);
				if (menuItem is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"Menu item with reference{menuId} not found"
					};
					response.IsSuccessful = false;
					return response;
				}
				response.IsSuccessful = true;
				response.Data = menuItem;
				return response;
			}
			catch (Exception ex)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = ex.Message
				};
				response.IsSuccessful = false;
				return response;
			}
		}
		public async Task<Responses> GetMenu(ClientParameters parameters, string cooperateId)
		{
			var response = new Responses(false);
			try
			{
				var menu =  _context.Menus.AsQueryable().Where(x => x.CooperateID == cooperateId);
				if (menu is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"No menu for cooperate with reference{cooperateId}"
					};
					response.IsSuccessful = false;
					return response;
				}
				var menuu = PagedList<Menu>.ToPagedList(menu,
				parameters.PageNumber,
				parameters.PageSize);
				response.IsSuccessful = true;
				response.Data = menuu;
				return response;
			}
			catch (Exception ex)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = ex.Message
				};
				response.IsSuccessful = false;
				return response;
			}
		}
	}
}
