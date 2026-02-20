using AutoMapper;
using TestiranjeAPI.Models;
using TestiranjeAPI.Models.Response;
using TestiranjeAPI.Repository;
using TestiranjeAPI.Repository.Interfaces;
using TestiranjeAPI.Services.Interfaces;

namespace TestiranjeAPI.Services;

public class TaskService : ITaskService
{

    private ITaskRepository _taskRepository;
    private IUserRepository _userRepository;
    private IPartyRepository _partyRepository;

    public TaskService(ITaskRepository tRepo, IUserRepository uRepo, IPartyRepository pRepo)
    {
        _taskRepository = tRepo;
        _userRepository = uRepo;
        _partyRepository = pRepo;
    }

    public async Task<List<UserTaskResponse>> GetUserTasks(int userId)
    {

        var existingUser = await _userRepository.GetByIdAsync(userId);

        if (existingUser == null) throw new Exception("User not found");

        return await _taskRepository.GetUserTasksAsync(userId);
    }

    public async Task CreateTask(TaskCreate task, int userId, int partyId)
    {

        var existingUser = await _userRepository.GetByIdAsync(userId);
        var existingParty = await _partyRepository.GetByIdAsync(partyId);

        if (existingParty == null || existingUser == null) throw new Exception("User or Party not found");

        Models.Special.Task newTask = new Models.Special.Task(task.Name, task.Description);
        newTask.User = existingUser!;
        newTask.Party = existingParty!;

        await _taskRepository.AddAsync(newTask);
        await _taskRepository.SaveChangesAsync();
    }

    public async Task EditTask(TaskUpdate task, int taskId)
    {

        var existingTask = await _taskRepository.GetByIdAsync(taskId);

        if (existingTask == null) throw new Exception("Task not found");

        existingTask.Name = task.Name;
        existingTask.Description = task.Description;

        _taskRepository.Update(existingTask);
        await _taskRepository.SaveChangesAsync();
    }

    public async Task RemoveTask(int taskId)
    {

        var taskToDelete = await _taskRepository.GetByIdAsync(taskId);

        if (taskToDelete == null) throw new Exception("Task not found");

        _taskRepository.Delete(taskToDelete);
        await _taskRepository.SaveChangesAsync();
    }


}