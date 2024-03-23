using EnterprisePortalWebAPI.Core.DTO;
using EnterprisePortalWebAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnterprisePortalWebAPI.Controllers
{
	[ApiController]
	[EnableCors("AllowMultipleOrigins")]
	public class BusinessController(IBusinessService service) : RootController
	{
		private readonly IBusinessService _service = service;

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] BusinessDTO request)
		{
			var result = await _service.Create(request);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[Authorize]
		[HttpPut()]
		public async Task<IActionResult> Update([FromBody] BusinessDTO request, [FromQuery] string businessId)
		{
			var result = await _service.Update(request, businessId);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpPost("verify")]
		public async Task<IActionResult> Verify([FromBody] VerificationDTO request, [FromQuery] string businessId)
		{
			var result = await _service.VerifyBusiness(request, businessId);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpPost("verification-update")]
		public async Task<IActionResult> Status([FromBody] UpdateVerificationStatus request)
		{
			var result = await _service.VerificationStageUpdate(request);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}

		[HttpGet()]
		public async Task<IActionResult> GetBusiness([FromQuery] string businessId)
		{
			var result = await _service.Get(businessId);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpDelete()]
		public async Task<IActionResult> DeleteBusiness([FromQuery] string businessId)
		{
			var result = await _service.Delete(businessId);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpGet("by-cooperate")]
		public IActionResult  GetUser([FromQuery] ClientParameters parameters, [FromQuery] string cooperateId)
		{
			var result = _service.GetByCooperateId(parameters, cooperateId);
			var metadata = new
			{
				result?.Data?.TotalCount,
				result?.Data?.PageSize,
				result?.Data?.CurrentPage,
				result?.Data?.TotalPages,
				result?.Data?.HasNext,
				result?.Data?.HasPrevious
			};
			Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metadata);
			if (result!.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
	}
}
