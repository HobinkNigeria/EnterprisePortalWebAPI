using AutoMapper;
using EnterprisePortalWebAPI.Core;
using EnterprisePortalWebAPI.Core.Domain;
using EnterprisePortalWebAPI.Core.DT;
using EnterprisePortalWebAPI.Core.DTO;
using EnterprisePortalWebAPI.Service.Interface;
using EnterprisePortalWebAPI.Utility;
using EnterprisePortalWebAPI.Utility.Services;
using Microsoft.EntityFrameworkCore;
namespace EnterprisePortalWebAPI.Service.Implementation
{
	public class UserService(DatabaseContext context, IMapper mapper, IJwtService jwtService, IEmailService emailService, IOneTimePasswordService oneTimePasswordService) : IUserService
	{
		private readonly DatabaseContext _context = context;
		private readonly IMapper _mapper = mapper;
		private readonly IJwtService _jwtService = jwtService;
		private readonly IEmailService _emailService = emailService;
		private readonly IOneTimePasswordService _oneTimePasswordService = oneTimePasswordService;
		public async Task<Responses> Create(AdminUserDTO request)
		{
			var responses = new Responses(false);
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower());
				if (user is not null)
				{
					responses.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.USER_EXIST,
						ResponseDescription = $"User with e-mail '{request.Email}' already exist, please contact the admin or try with a different e-mail address"
					};
					responses.IsSuccessful = false;
					return responses;
				}
				var validation =await _oneTimePasswordService.ValidateOTP(new ValidateOneTimePasswordDTO() { Email = request.Email, OTP = request.OTP, Purpose = request.Purpose });
				if (!validation.IsSuccessful)
					return validation;

				var jwtToken = await _jwtService.GenerateToken(request.Email);
				var createdUser = await CreateUser(_mapper.Map<UserDTO>(request), false);

				var responsePayload = _mapper.Map<LoginResponseDTO>(createdUser);
				responsePayload.Token = jwtToken.JwtToken;

				responses.IsSuccessful = true;
				responses.Data = responsePayload;
				return responses;
			}
			catch (Exception)
			{
				responses.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = "Operation failed, kindly retry"
				};
				responses.IsSuccessful = false;
				return responses;
			}
		}

		private async Task<User> CreateUser(UserDTO request, bool isAdditionalAccount)
		{
			var userToCreate = _mapper.Map<User>(request);
			userToCreate.CooperateID = isAdditionalAccount ? request.CooperateID : Guid.NewGuid().ToString();
			userToCreate.DateCreated = DateTime.Now;
			userToCreate.DateUpdated = DateTime.Now;
			userToCreate.Password = Util.HashPassword(request.Password);

			_context.Add(userToCreate);
			await _context.SaveChangesAsync();
			return userToCreate;
		}

		public async Task<Responses> CreateAdditionalAccount(UserDTO request)
		{
			var responses = new Responses(false);
			try
			{
				var cooperate = await _context.Users.FirstOrDefaultAsync(x => x.CooperateID == request.CooperateID);
				if (cooperate is null)
				{
					responses.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.USER_EXIST,
						ResponseDescription = "Cooperate Id not found"
					};
					responses.IsSuccessful = false;
					return responses;
				}
				else
				{
					var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower());
					if (user is not null)
					{
						responses.Error = new ErrorResponse
						{
							ResponseCode = ResponseCodes.USER_EXIST,
							ResponseDescription = $"User with e-mail '{request.Email}' already exist, please contact the admin or try with a different e-mail address"
						};
						responses.IsSuccessful = false;
						return responses;
					}
					await CreateUser(request, true);
					responses.IsSuccessful = true;
					responses.Data = "Successfully completed";
					return responses;
				}
			}
			catch (Exception)
			{
				responses.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = "Operation failed, kindly retry"
				};
				responses.IsSuccessful = false;
				return responses;
			}
		}
		public async Task<Responses> Update(UserDTO request, string userId)
		{
			var response = new Responses(false);
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
				if (user is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"User '{request.FirstName}-{request.LastName}' not found"
					};
					response.IsSuccessful = false;
					return response;
				}
				var userToUpdate = _mapper.Map(request, user);
				userToUpdate.DateUpdated = DateTime.Now;
				userToUpdate.Password = user.Password;

				_context.Update(userToUpdate);
				await _context.SaveChangesAsync();

				response.IsSuccessful = true;
				response.Data = "Successfully completed";
				return response;
			}
			catch (Exception)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = "Operation failed, kindly retry"
				};
				response.IsSuccessful = false;
				return response;
			}
		}
		public async Task<Responses> UpdatePassword(ChangePasswordDTO request)
		{
			var response = new Responses(false);
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower());
				if (user is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = "User not found"
					};
					response.IsSuccessful = false;
					return response;
				}
				bool isValidPassword = Util.VerifyPassword(request.OldPassword, user.Password);
				if (!isValidPassword)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = "Old password is incorrect"
					};
					response.IsSuccessful = false;
					return response;
				}
				user.Password = Util.HashPassword(request.NewPassword);
				user.PasswordLastChanged = DateTime.Now;
				user.DateUpdated = DateTime.Now;

				_context.Update(user);
				await _context.SaveChangesAsync();

				response.IsSuccessful = true;
				response.Data = "Successfully completed";
				return response;
			}
			catch (Exception)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = "Operation failed, kindly retry"
				};
				response.IsSuccessful = false;
				return response;
			}
		}
		public async Task<Responses> Delete(string userId)
		{
			var response = new Responses(false);
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
				if (user is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"User with reference '{userId}' not found"
					};
					response.IsSuccessful = false;
					return response;
				}

				_context.Remove(user);
				await _context.SaveChangesAsync();

				response.IsSuccessful = true;
				response.Data = "Successfully completed";
				return response;
			}
			catch (Exception)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = "Operation failed, kindly retry"
				};
				response.IsSuccessful = false;
				return response;
			}
		}
		public async Task<Responses> Get(string userId)
		{
			var response = new Responses(false);
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
				if (user is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"User with reference '{userId}' not found"
					};
					response.IsSuccessful = false;
					return response;
				}
				response.IsSuccessful = true;
				response.Data = _mapper.Map<UserResponseDTO>(user);
				return response;
			}
			catch (Exception)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = "Operation failed, kindly retry"
				};
				response.IsSuccessful = false;
				return response;
			}
		}
		public Responses GetbyCooperate(ClientParameters parameters, string cooperateId)
		{
			var response = new Responses(false);
			try
			{
				var users = _context.Users.Where(x => x.CooperateID == cooperateId)
					.Select(x => _mapper.Map<UserResponseDTO>(x));

				var result = PagedList<UserResponseDTO>.ToPagedList(users,
				parameters.PageNumber,
				parameters.PageSize);

				response.IsSuccessful = true;
				response.Data = result;
				return response;
			}
			catch (Exception)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = "Operation failed, kindly retry"
				};
				response.IsSuccessful = false;
				return response;
			}
		}
		public Responses GetByBusinessId(ClientParameters parameters, string cooperateId, string businessId)
		{
			var response = new Responses(false);
			try
			{
				var users = _context.Users.Where(x => x.CooperateID == cooperateId && x.BusinessID == businessId)
					.Select(x => _mapper.Map<UserResponseDTO>(x));

				var result = PagedList<UserResponseDTO>.ToPagedList(users,
				parameters.PageNumber,
				parameters.PageSize);

				response.IsSuccessful = true;
				response.Data = result;
				return response;
			}
			catch (Exception)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = "Operation failed, kindly retry"
				};
				response.IsSuccessful = false;
				return response;
			}
		}
		public async Task<Responses> Login(LoginDTO request)
		{
			var response = new Responses(false);
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower());
				if (user is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"User with e-mail '{request.Email}' not found"
					};
					response.IsSuccessful = false;
					return response;
				}
				bool isValidPassword = Util.VerifyPassword(request.Password, user.Password);
				if (!isValidPassword)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = "Password is incorrect"
					};
					response.IsSuccessful = false;
					return response;
				}
				if (user.PasswordIsSystemGenerated)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.SYSTEM_PASSWORD,
						ResponseDescription = $"Kindly update your passowrd to login"
					};
					response.IsSuccessful = false;
					return response;
				}
				var jwtToken = await _jwtService.GenerateToken(user.Email);

				var responsePayload = _mapper.Map<LoginResponseDTO>(user);
				responsePayload.Token = jwtToken.JwtToken;

				if (user.Role != Core.Enum.UserRole.Operator)
					responsePayload.Businesses = _context.Businesses.Where(x => x.CooperateID == user.CooperateID).Select(x => new BuinessAvailable() { BusinessId = x.Id, BusinessName = x.Name }).ToList();

				user.LastLogin = DateTime.Now;
				_context.Update(user);
				await _context.SaveChangesAsync();

				response.IsSuccessful = true;
				response.Data = responsePayload;
				return response;
			}
			catch (Exception)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = "Operation failed, kindly retry"
				};
				response.IsSuccessful = false;
				return response;
			}
		}
		public async Task<Responses> RefreshLoginToken(RefreshTokenReqDTO request)
		{
			var response = new Responses(false);
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower());
				if (user is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"User with e-mail '{request.Email}' not found"
					};
					response.IsSuccessful = false;
					return response;
				}
				return await _jwtService.RefreshToken(request);
			}
			catch (Exception)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = "Operation failed, kindly retry"
				};
				response.IsSuccessful = false;
				return response;
			}
		}
		public async Task<Responses> ForgetPassword(string email)
		{
			var response = new Responses(false);
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
				if (user is null)
				{
					response.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.REQUEST_NOT_FOUND,
						ResponseDescription = $"User with e-mail '{email}' not found"
					};
					response.IsSuccessful = false;
					return response;
				}
				var password = Util.GenerateRandomPassword(10);
				user.Password = Util.HashPassword(password);
				user.PasswordIsSystemGenerated = true;
				user.PasswordLastChanged = DateTime.Now;
				user.DateUpdated = DateTime.Now;
				string subject = "Forget Password";
				_emailService.SendEmail(email, subject, EmailTemplate.GetPasswordChangeTemplate(subject, password));

				_context.Update(user);
				await _context.SaveChangesAsync();

				response.IsSuccessful = true;
				response.Data = "Successfully completed";
				return response;

			}
			catch (Exception)
			{
				response.Error = new ErrorResponse
				{
					ResponseCode = ResponseCodes.GENERAL_ERROR,
					ResponseDescription = "Operation failed, kindly retry"
				};
				response.IsSuccessful = false;
				return response;
			}
		}
	}
}
