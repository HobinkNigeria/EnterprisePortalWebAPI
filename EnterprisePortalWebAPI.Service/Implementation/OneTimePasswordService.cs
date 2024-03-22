using EnterprisePortalWebAPI.Core;
using EnterprisePortalWebAPI.Core.Domain;
using EnterprisePortalWebAPI.Core.DT;
using EnterprisePortalWebAPI.Core.DTO;
using EnterprisePortalWebAPI.Service.Interface;
using EnterprisePortalWebAPI.Utility.Services;
using Microsoft.EntityFrameworkCore;
namespace EnterprisePortalWebAPI.Service.Implementation
{
	public class OneTimePasswordService(DatabaseContext context, IEmailService emailService) : IOneTimePasswordService
	{
		private readonly DatabaseContext _context = context;
		private readonly IEmailService _emailService = emailService;

		public async Task<Responses> GenerateOTP(OneTimePasswordDTO request)
		{
			var responses = new Responses(false);
			try
			{
				var otpSent = await _context.OneTimePasswords.FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower() && x.Purpose == request.Purpose && x.IsUsed == false);
				if (otpSent is not null && otpSent.ExpiryTime >= DateTime.Now)
				{
					responses.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.INPUT_VALIDATION_FAILURE,
						ResponseDescription = $"OTP has been sent to user with e-mail '{request.Email}', please verify with sent OTP"
					};
					responses.IsSuccessful = false;
					return responses;
				}
				string otp = GenerateRandomOTP();
				DateTime creationTime = DateTime.Now;
				DateTime expiryTime = creationTime.AddMinutes(5);

				_emailService.SendEmail(otp, request.Email, request.Purpose.ToString());

				var otpToRecord = new OneTimePassword()
				{
					Email = request.Email,
					OTP = otp,
					DateCreated = creationTime,
					ExpiryTime = expiryTime,
					IsUsed = false,
					Purpose = request.Purpose
				};
				_context.Add(otpToRecord);
				await _context.SaveChangesAsync();

				responses.IsSuccessful = true;
				responses.Data = "Successfully completed";
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
		public async Task<Responses> ValidateOTP(ValidateOneTimePasswordDTO request)
		{
			var responses = new Responses(false);
			try
			{
				var otpSent = await _context.OneTimePasswords.FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower() && x.Purpose == request.Purpose);
				if (otpSent is null)
				{
					responses.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.INPUT_VALIDATION_FAILURE,
						ResponseDescription = $"OTP not sent for {request.Purpose} for user with user with email {request.Email}"
					};
					responses.IsSuccessful = false;
					return responses;
				}

				if (otpSent.OTP.ToLower() != request.OTP.ToLower())
				{
					responses.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.INPUT_VALIDATION_FAILURE,
						ResponseDescription = $"Incorrect OTP provided"
					};
					responses.IsSuccessful = false;
				}
				else if (otpSent.ExpiryTime >= DateTime.Now)
				{
					responses.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.INPUT_VALIDATION_FAILURE,
						ResponseDescription = $"OTP has expired"
					};
					responses.IsSuccessful = false;
				}
				else if (otpSent.IsUsed)
				{
					responses.Error = new ErrorResponse
					{
						ResponseCode = ResponseCodes.INPUT_VALIDATION_FAILURE,
						ResponseDescription = $"OTP has been used and is not valid"
					};
					responses.IsSuccessful = false;
				}
				else
				{
					responses.IsSuccessful = true;
					responses.Data = "Successfully validated";
				}
				_context.Remove(otpSent);
				await _context.SaveChangesAsync();

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
		private static string GenerateRandomOTP()
		{
			Random random = new();
			return random.Next(100000, 999999).ToString();
		}
	}
}
