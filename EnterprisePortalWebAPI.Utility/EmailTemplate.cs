
namespace EnterprisePortalWebAPI.Utility
{
	public static class EmailTemplate
	{
		public static string GetTemplate(string subject, string code)
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
		public static string GetPasswordChangeTemplate(string subject, string code)
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
              <p>Welcome To Hobink Paasword Manager!</p>
          </div>
          <div class=""content"">
              <p>This email is to inform you that a new password has been generated for your account. This password has been created for security reasons, and you will need to change it upon your next login to access the application.</p>
              <p>Temporary Password: {code}</p>
              <p>Please remember to keep this password confidential and do not share it with anyone. For your security, it is important to change your password regularly</p>
              <p> To access the application and change your password, please follow these steps:</p>
              <p>1. Go to the login page</p>
              <p> 2. Enter your email associated with your account.</p>
              <p> 3. Use the temporary password provided above to log in.</p>
              <p> 4. Upon successful login, you will be prompted to create a new password. Please choose a strong and secure password that you can remember easily.</p>
              <p> If you encounter any difficulties or have any questions regarding this process, please do not hesitate to contact our support team at the contact below</p>
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