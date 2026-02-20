using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeAPI.Models;
using TestiranjeAPI.Models.Request;
using TestiranjeAPI.Models.Response;
using TestiranjeAPI.Services.Interfaces;

namespace TestiranjeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PartyController : ControllerBase
{
    private IUserService _userService;
    private IPartyService _partyService;
    public PartyController(IUserService userService, IPartyService partyService)
    {
        _userService = userService;
        _partyService = partyService;
    }

    #region GET_REQUESTS
    [HttpGet("my-parties/{userId}")]
    public async Task<ActionResult> GetUserCreatedParties([FromRoute] int userId)
    {
        try
        {
            var userParties = await _partyService.GetUserParty(userId);
            return Ok(userParties);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("available-parties")]
    public async Task<ActionResult> GetAllParties()
    {
        try
        {
            var availableParties = await _partyService.GetAllParties();
            return Ok(availableParties);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("my-attending-parties/{userId}")]
    public async Task<ActionResult<List<Party>>> GetUserAttendingParties([FromRoute] int userId)
    {
        try
        {
            var userAttendingParties = await _partyService.GetUserAttendingParties(userId);
            return Ok(userAttendingParties);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("parties-names/{userId}")]
    public async Task<ActionResult<List<PartyNameIdResponse>>> GetUserCreatedPartiesNames([FromRoute] int userId)
    {
        try
        {
            var parties = await _partyService.GetUserCreatedPartiesNames(userId);
            return Ok(parties);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    #endregion

    #region POST_REQUESTS
    [HttpPost("create/{userId}")]
    public async Task<ActionResult> CreateParty([FromBody] PartyCreateRequest party, [FromRoute] int userId)
    {
        try
        {
            await _partyService.CreateParty(party, userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("attend/{partyId}/{userId}")]
    public async Task<ActionResult> AttendParty([FromRoute] int partyId, [FromRoute] int userId)
    {
        try
        {
            await _partyService.AttendParty(partyId, userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    #endregion

    #region DELETE_REQUESTS

    [HttpDelete("unattend/{partyId}/{userId}")]
    public async Task<ActionResult> CancelUserAttendance([FromRoute] int partyId, [FromRoute] int userId)
    {
        try
        {
            await _partyService.CancelUserAttendance(partyId, userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("cancel/{partyId}")]
    public async Task<ActionResult> CancelUserParty([FromRoute] int partyId)
    {
        try
        {
            await _partyService.CancelUserParty(partyId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    #endregion

    #region PUT_REQUESTS

    [HttpPut("edit/{partyId}")]
    public async Task<ActionResult> EditParty([FromBody] PartyUpdate partyUpdate, [FromRoute] int partyId)
    {
        try
        {
            await _partyService.EditParty(partyUpdate, partyId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    #endregion
}
