namespace EnterprisePortalWebAPI.Core.Domain
{
	public class Menu : BaseModel
	{
		public required string Item { get; set; }
		public string ItemDescription { get; set; } = String.Empty;
		public decimal Price { get; set; }
		public string Currency { get; set; } = "NGA";
		public required string CooperateID { get; set; }
	}
}
