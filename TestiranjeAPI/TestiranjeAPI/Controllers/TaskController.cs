using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeAPI.Models;
using TestiranjeAPI.Services.Interfaces;

namespace TestiranjeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private ITaskService _taskService;
    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;

    }

    [HttpGet("my-tasks/{userId}")]
    public async Task<ActionResult> GetUserTasks([FromRoute] int userId)
    {
        try
        {
            var userTasks = await _taskService.GetUserTasks(userId);
            return Ok(userTasks);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("create/{userId}/{partyId}")]
    public async Task<ActionResult> CreateTask([FromBody] TaskCreate task, [FromRoute] int userId, [FromRoute] int partyId)
    {
        try
        {
            await _taskService.CreateTask(task, userId, partyId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("edit/{taskId}")]
    public async Task<ActionResult> EditTask([FromBody] TaskUpdate task, [FromRoute] int taskId)
    {
        try
        {
            await _taskService.EditTask(task, taskId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("remove/{taskId}")]
    public async Task<ActionResult> RemoveTask([FromRoute] int taskId)
    {
        try
        {
            await _taskService.RemoveTask(taskId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
