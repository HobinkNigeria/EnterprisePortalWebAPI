using EnterprisePortalWebAPI.Core.Enum;
using System.ComponentModel;

namespace EnterprisePortalWebAPI.Core.DTO
{
	public class BasicResponse
	{
		[DefaultValue(true)]
		public bool IsSuccessful { get; set; }
		[DefaultValue(null)]
		public ErrorResponse? Error { get; set; }

		public BasicResponse()
		{
			IsSuccessful = false;
		}
		public BasicResponse(bool isSuccessful)
		{
			IsSuccessful = isSuccessful;
		}
		[DefaultValue(FaultMode.NONE)]
		public FaultMode FaultType { get; set; }
	}
}
