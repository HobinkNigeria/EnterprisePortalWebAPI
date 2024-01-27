using EnterprisePortalWebAPI.Controllers;
using EnterprisePortalWebAPI.Core.DTO;
using EnterprisePortalWebAPI.Service.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MTNVoiceOfCustomer.Controllers
{
	[ApiController]
	[EnableCors("AllowMultipleOrigins")]

	public class MenuController : RootController
	{
		private readonly IMenuService _service;
		public MenuController(IMenuService service)
		{
			_service = service;
		}
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] MenuDTO request)
		{
			var result = await _service.Create(request);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] MenuDTO request, [FromQuery] string menuId)
		{
			var result = await _service.Update(request, menuId);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] string menuId)
		{
			var result = await _service.Get(menuId);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpGet("by-cooparete")]
		public async Task<IActionResult> GetAudiences([FromQuery] ClientParameters parameters, [FromQuery] string cooperateId)
		{
			var result = await _service.GetMenu(parameters, cooperateId);
			var metadata = new
			{
				result.Data?.TotalCount,
				result.Data?.PageSize,
				result.Data?.CurrentPage,
				result.Data?.TotalPages,
				result.Data?.HasNext,
				result.Data?.HasPrevious
			};
			Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpDelete]
		public async Task<IActionResult> Remove([FromQuery] string audienceId)
		{
			var result = await _service.Delete(audienceId);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
	}
}
