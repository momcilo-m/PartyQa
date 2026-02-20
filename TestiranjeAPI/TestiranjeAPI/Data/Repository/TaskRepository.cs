using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using TestiranjeAPI.Models;
using TestiranjeAPI.Models.Response;
using TestiranjeAPI.Repository.Interfaces;

namespace TestiranjeAPI.Repository;

public class TaskRepository : Repository<Models.Special.Task>, ITaskRepository
{
    public TaskRepository(PartyContext context) : base(context) { }

    public async Task<List<UserTaskResponse>> GetUserTasksAsync(int userId)
    {
        var tasks = await _context.Tasks
            .Include(t => t.User)
            .Include(t => t.Party)
            .Where(t => t.User.Id == userId)
            .GroupBy(t => new { t.Party.Id, t.Party.Name })
            .Select(group => new UserTaskResponse
            (group.Key.Id, group.Key.Name, group.Select(t => new TaskDataResponse(t.Id, t.Name, t.Description))))
            .ToListAsync();

        return tasks;

    }

    public async System.Threading.Tasks.Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
        return;
    }
}
