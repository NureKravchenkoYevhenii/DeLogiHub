using BLL.Contracts;
using BLL.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeLogiHubApi.Controllers;
[Area("users")]
[Route("api/[area]")]
[ApiController]
public class UsersController : DeLogiHubBaseController
{
	private readonly IUserService _userService;

	public UsersController(
		IUserService userService)
	{
		_userService = userService;
	}

	[HttpPost("register")]
	public ActionResult Register([FromBody] RegisterUserModel registerModel)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		_userService.RegisterUser(registerModel);

		return Ok();
	}

	[HttpPost("update")]
	[Authorize]
	public IActionResult UpdateUserProfile([FromBody] UserProfileInfo userModel)
	{
		if (!ModelState.IsValid)
			return BadRequest(userModel);

		_userService.UpdateUserProfile(userModel);

		return Ok();
	}

	[HttpGet("get")]
	[Authorize]
	[ProducesResponseType(typeof(UserProfileModel), (int)HttpStatusCode.OK)]
	public IActionResult GetUserProfileById([FromQuery] Guid id)
	{
		var user = _userService.GetUserProfileById(id);

		return Ok(user);
	}
}
