using System.Net;
using EnterprisePortalWebAPI.Core.DTO;
using EnterprisePortalWebAPI.Core.Enum;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EnterprisePortalWebAPI.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[EnableCors("AllowAny")]
	public class RootController : ControllerBase
	{
		protected IActionResult CreateResponse(ErrorResponse error, FaultMode category)
		{
			var code = category switch
			{
				FaultMode.UNAUTHORIZED => (int)HttpStatusCode.Unauthorized,
				FaultMode.CLIENT_INVALID_ARGUMENT => (int)HttpStatusCode.BadRequest,
				FaultMode.INVALID_OBJECT_STATE => (int)HttpStatusCode.Conflict,
				FaultMode.GATEWAY_ERROR => (int)HttpStatusCode.BadGateway,
				FaultMode.REQUESTED_ENTITY_NOT_FOUND => (int)HttpStatusCode.NotFound,
				_ => (int)HttpStatusCode.InternalServerError,
			};
			return new ObjectResult(error)
			{
				StatusCode = code
			};
		}
	}
}
