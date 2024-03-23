namespace EnterprisePortalWebAPI.Utility.Services
{
	public interface IEmailService
	{
		bool SendEmail(string code, string toEmail, string subject);
	}
}
