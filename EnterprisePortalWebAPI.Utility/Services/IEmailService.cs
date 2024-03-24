namespace EnterprisePortalWebAPI.Utility.Services
{
	public interface IEmailService
	{
		bool SendEmail(string toEmail, string subject,string body);
	}
}
