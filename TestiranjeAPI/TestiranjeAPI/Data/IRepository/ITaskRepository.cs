using TestiranjeAPI.Models;
using TestiranjeAPI.Models.Response;
using Task = System.Threading.Tasks.Task;

namespace TestiranjeAPI.Repository.Interfaces;

public interface ITaskRepository : IRepository<Models.Special.Task>
{
    Task<List<UserTaskResponse>> GetUserTasksAsync(int userId);
    Task SaveChangesAsync();
}
