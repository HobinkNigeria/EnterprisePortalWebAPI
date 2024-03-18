﻿using EnterprisePortalWebAPI.Core.DTO;
using EnterprisePortalWebAPI.Service.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnterprisePortalWebAPI.Controllers
{
	[ApiController]
	[EnableCors("AllowMultipleOrigins")]
	public class UserController(IUserService service) : RootController
	{
		private readonly IUserService _service = service;

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO request)
		{
			var result = await _service.Login(request);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}

		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenReqDTO request)
		{
			var result = await _service.RefreshLoginToken(request);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpPost("change-password")]
		public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordDTO request)
		{
			var result = await _service.UpdatePassword(request);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpPost()]
		public async Task<IActionResult> Create([FromQuery] UserDTO request)
		{
			var result = await _service.Create(request,false);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpPost("additional-account")]
		public async Task<IActionResult> CreateAdditionalAccount([FromQuery] UserDTO request)
		{
			var result = await _service.CreateAdditionalAccount(request);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateUser([FromBody] UserDTO request, [FromQuery] string UserId)
		{
			var result = await _service.Update(request, UserId);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}

		[HttpGet()]
		public async Task<IActionResult> GetUser([FromQuery] string UserId)
		{
			var result = await _service.Get(UserId);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpDelete()]
		public async Task<IActionResult> DeleteUser([FromQuery] string UserId)
		{
			var result = await _service.Delete(UserId);
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
		[HttpGet("by-cooperate")]
		public IActionResult  GetUser([FromQuery] ClientParameters parameters, [FromQuery] string cooperateId)
		{
			var result = _service.GetbyCooperate(parameters, cooperateId);
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
			if (result.IsSuccessful)
				return Ok(result);
			return BadRequest(result);
		}
	}
}
