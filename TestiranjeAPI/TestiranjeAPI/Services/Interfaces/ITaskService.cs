using TestiranjeAPI.Models;
using TestiranjeAPI.Models.Response;

namespace TestiranjeAPI.Services.Interfaces;

public interface ITaskService
{
    public Task<List<UserTaskResponse>> GetUserTasks(int userId);
    public Task CreateTask(TaskCreate task, int userId, int partyId);
    public Task EditTask(TaskUpdate task, int taskId);
    public Task RemoveTask(int taskId);
}