using Microsoft.AspNetCore.Mvc;
using TestiranjeAPI.Models;
using TestiranjeAPI.Services.Interfaces;

namespace TestiranjeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("signup")]
    public async Task<ActionResult> UserSignup([FromBody] UserRegister userRegister)
    {
        try
        {
            await _userService.Register(userRegister);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> UserLogin([FromBody] UserLogin userLogin)
    {
        try
        {
            int id = await _userService.Login(userLogin);
            return Ok(id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("update/{userId}")]
    public async Task<ActionResult> UserUpdate([FromBody] UserUpdate userUpdate, [FromRoute] int userId)
    {
        try
        {
            await _userService.UpdateUser(userId, userUpdate);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("info/{id}")]
    public async Task<ActionResult> GetUserInfo([FromRoute] int id)
    {
        try
        {
            var user = await _userService.GetUserInfo(id);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
