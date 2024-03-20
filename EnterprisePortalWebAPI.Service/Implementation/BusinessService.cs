using AutoMapper;
using EnterprisePortalWebAPI.Core;
using EnterprisePortalWebAPI.Core.Domain;
using EnterprisePortalWebAPI.Core.DT;
using EnterprisePortalWebAPI.Core.DTO;
using Microsoft.EntityFrameworkCore;

namespace EnterprisePortalWebAPI.Service.Implementation
{
	public class BusinessService(DatabaseContext context, IMapper mapper) 
	{
		private readonly DatabaseContext _context = context;
		private readonly IMapper _mapper = mapper;
		public async Task<Responses> Create(BusinessDTO request)
		{
			var responses = new Responses(false);
			try
			{
				var business = await _context.Businesses.FirstOrDefaultAsync(x => x.Name.ToLower() == request.Name.ToLower() && x.CooperateID == request.CooperateID);

				if (business is not null)
				{
					responses.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.BUSINESS_EXIST,
						ResponseDescription = $"Busniess with name '{request.Name}' has already been created"
					};
					responses.IsSuccessful = false;
					return responses;
				}

				var businessToCreate = _mapper.Map<Business>(request);
				businessToCreate.DateCreated = DateTime.Now;
				businessToCreate.DateUpdated = DateTime.Now;

				_context.Add(businessToCreate);
				await _context.SaveChangesAsync();

				responses.IsSuccessful = true;
				responses.Data = businessToCreate;
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

		public async Task<Responses> Update(BusinessDTO request, string businessId)
		{
			var response = new Responses(false);
			try
			{
				var business = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == businessId);
				if (business is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"Business with reference '{businessId}' not found"
					};
					response.IsSuccessful = false;
					return response;
				}
				var businessToUpdate = _mapper.Map(request, business);
				businessToUpdate.DateUpdated = DateTime.Now;

				_context.Update(businessToUpdate);
				await _context.SaveChangesAsync();

				response.IsSuccessful = true;
				response.Data = businessToUpdate;
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
		public async Task<Responses> Delete(string businessId)
		{
			var response = new Responses(false);
			try
			{
				var business = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == businessId);
				if (business is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"Business with reference '{businessId}' not found"
					};
					response.IsSuccessful = false;
					return response;
				}

				_context.Remove(business);
				await _context.SaveChangesAsync();

				response.IsSuccessful = true;
				response.Data = business;
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
		public async Task<Responses> Get(string businessId)
		{
			var response = new Responses(false);
			try
			{
				var business = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == businessId);
				if (business is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"Business with reference '{businessId}' not found"
					};
					response.IsSuccessful = false;
					return response;
				}
				response.IsSuccessful = true;
				response.Data = business;
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
