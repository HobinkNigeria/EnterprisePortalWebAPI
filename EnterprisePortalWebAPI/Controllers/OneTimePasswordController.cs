using EnterprisePortalWebAPI.Core.DTO;
using EnterprisePortalWebAPI.Service.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EnterprisePortalWebAPI.Controllers
{
	[ApiController]
	[EnableCors("AllowMultipleOrigins")]
	public class OneTimePasswordController(IOneTimePasswordService service) : RootController
	{
		private readonly IOneTimePasswordService _service = service;

		[HttpPost("generate")]
		public async Task<IActionResult> Generate([FromBody] OneTimePasswordDTO request)
		{
			var result = await _service.GenerateOTP(request);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpPost("validate")]
		public async Task<IActionResult> Verify([FromBody] ValidateOneTimePasswordDTO request)
		{
			var result = await _service.ValidateOTP(request);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}	
	}
}
