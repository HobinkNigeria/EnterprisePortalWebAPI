using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace EnterprisePortalWebAPI.Utility.Services
{
    public class EmailService(IConfiguration config) : IEmailService
	{
        private readonly string server = config.GetSection("ElasticEmailSettings").GetSection("Server").Value!;
        private readonly string smtpPassword = config.GetSection("ElasticEmailSettings").GetSection("Password").Value!;
        private readonly string smtpUsername = config.GetSection("ElasticEmailSettings").GetSection("UserName").Value!;
        private readonly int port = Convert.ToInt32(config.GetSection("ElasticEmailSettings").GetSection("Port").Value!);
        public bool SendEmail(string code, string toEmail, string subject)
        {
            string body = EmailTemplate.GetTemplate(subject, code);

            SmtpClient client = new(server)
            {
                Port = port,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            MailMessage mailMessage = new(smtpUsername, toEmail, subject, body)
            {
                IsBodyHtml = true
            };

            try
            {
                client.Send(mailMessage);
                Console.WriteLine("Email sent successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
            finally
            {
                client.Dispose();

            }
            return false;
        }
    }
}
