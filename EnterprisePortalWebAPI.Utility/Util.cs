using Newtonsoft.Json;
using System.Text.RegularExpressions;
namespace EnterprisePortalWebAPI.Utility
{
	public static partial class Util
	{
		static readonly JsonSerializerSettings settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore };

		public static string HashPassword(string password)
		=> BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
		public static bool VerifyPassword(string password, string hashedPassword)
		=> BCrypt.Net.BCrypt.Verify(password, hashedPassword);
		public static bool IsEmail(string input)
		=> MyRegex().IsMatch(input);
		public static string MaskPhoneNumber(string phonenumber)
		{
			if (phonenumber.Length > 3)
			{
				var lastDigits = phonenumber.Substring(phonenumber.Length - 4, 4);
				phonenumber = string.Concat(new String('*', phonenumber.Length - lastDigits.Length), lastDigits);
			}
			return phonenumber;
		}
		public static string SerializeAsJson<T>(T item)
		=> JsonConvert.SerializeObject(item);

		public static T DeserializeFromJson<T>(string input)
		=> JsonConvert.DeserializeObject<T>(input, settings)!;

		[GeneratedRegex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
		private static partial Regex MyRegex();
	}
}
