namespace EnterprisePortalWebAPI.Core.DTO
{
	public class LanguagePack(string defaultMessage)
	{
		public string? DefaultMessage { get; set; } = defaultMessage;
		public IDictionary<string, string>? Mappings { get; set; }
	}
}
