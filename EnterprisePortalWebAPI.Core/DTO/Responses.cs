namespace EnterprisePortalWebAPI.Core.DTO
{
	public class Responses(bool issuccess)
	{
		public dynamic? Data { get; set; }
		public ErrorResponse? Error { get; set; }
		public bool IsSuccessful { get; set; } = issuccess;
	}
}
