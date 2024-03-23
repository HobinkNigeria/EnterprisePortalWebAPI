
namespace EnterprisePortalWebAPI.Utility
{
	public static class EmailTemplate
	{
		public static string GetTemplate(string subject ,string code)
			=> $@"
  <!DOCTYPE html>
  <html lang=""en"">
  <head>
      <meta charset=""UTF-8"">
      <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
      <title>Hobink - {subject}</title>
      <style>
          /* Reset CSS */
          body, body * {{
              margin: 0;
              padding: 0;
              box-sizing: border-box;
          }}

          /* Main Styles */
          .container {{
              max-width: 600px;
              margin: 0 auto;
              padding: 20px;
              font-family: Arial, sans-serif;
              background-color: #f9f9f9;
              border-radius: 10px;
          }}

          .header {{
              background-color: #6115a8;
              color: #fff;
              padding: 20px;
              text-align: center;
              border-radius: 10px 10px 0 0;
          }}

          .header h1 {{
              font-size: 24px;
          }}

          .content {{
              background-color: #fff;
              padding: 20px;
              border-radius: 0 0 10px 10px;
          }}

          .footer {{
              background-color: #6115a8;
              color: #fff;
              padding: 20px;
              text-align: center;
              border-radius: 0 0 10px 10px;
          }}

          .footer p {{
              font-size: 14px;
              margin-top: 10px;
          }}

          .button {{
              display: inline-block;
              padding: 10px 20px;
              background-color: #6115a8;
              color: #fff;
              text-decoration: none;
              border-radius: 5px;
          }}

          .button:hover {{
              background-color: #45a049;
          }}
      </style>
  </head>
  <body>
      <div class=""container"">
          <div class=""header"">
              <h1>{subject}</h1>
              <p>Welcome To Hobink One Time Password!</p>
          </div>
          <div class=""content"">
              <p>Please note that this email contains a one-time password (OTP) for authentication purposes. The OTP is valid for a limited time and is intended for use only during the current authentication process.</p>
              <p>If you did not request this OTP or if you suspect any unauthorized activity, please contact our support team immediately at the contact below.</p>
              <p style=""text-align: center; font-weight: bold;"">{code}</p>
          </div>
          <div class=""footer"">
              <p>&copy; 2024 Hobink Global Services. All rights reserved.</p>
              <p>Contact us: info@Hobink.com</p>
          </div>
      </div>
  </body>
  </html>";

	}
}
