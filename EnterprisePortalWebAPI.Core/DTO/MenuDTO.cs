namespace EnterprisePortalWebAPI.Core.DTO
{
	public class MenuDTO
	{
		public required string Item { get; set; }
		public required string CooperateID { get; set; }
		public string? ItemDescription { get; set; }
		public decimal Price { get; set; }
		public string? Currency { get; set; }
	}
}
