using TestiranjeAPI.Models;
using TestiranjeAPI.Models.Request;
using TestiranjeAPI.Services;
using Task = System.Threading.Tasks.Task;

namespace TestiranjeAPI.Component.Tests;

[TestFixture]
public class TaskServiceComponentTests : BaseComponentTest
{
    private TaskService _taskService = null!;
    private const string TestPrefix = "nunit_test_";

    public override void Setup()
    {
        base.Setup();
        _taskService = new TaskService(TaskRepository, UserRepository, PartyRepository);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        // Obriši sve test zadatke
        var testTasks = await Context.Tasks
            .Where(t => t.Name.StartsWith(TestPrefix))
            .ToListAsync();

        foreach (var task in testTasks)
        {
            TaskRepository.Delete(task);
            await TaskRepository.SaveChangesAsync();
        }

        // Obriši sve test zabave sa "nunit_test_" prefixom
        var testParties = await Context.Parties
            .Where(p => p.Name.StartsWith(TestPrefix))
            .ToListAsync();

        foreach (var party in testParties)
        {
            PartyRepository.Delete(party);
            await PartyRepository.SaveChangesAsync();

        }

        // Obriši sve test korisnike sa "nunit_test_" prefixom
        var testUsers = await Context.Users
            .Where(u => u.Username.StartsWith(TestPrefix))
            .ToListAsync();

        foreach (var user in testUsers)
        {
            UserRepository.Delete(user);
            await UserRepository.SaveChangesAsync();

        }

        await TaskRepository.SaveChangesAsync();
    }

    #region CreateTask Tests (CREATE)

    [Test]
    public async Task CreateTask_WithValidData_ShouldCreateTaskSuccessfully()
    {
        // Arrange
        var user = await CreateTestUserAsync("nunit_test_task_user_1", "nunit_test_taskuser1@test.com", "nunit_test_passTaskUser1", "nunit_test_avatarTaskUser1.jpg");
        var creator = await CreateTestUserAsync("nunit_test_task_creator_1", "nunit_test_taskcreator1@test.com", "nunit_test_passTaskCreator1", "nunit_test_avatarTaskCreator1.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_task_party_1", "Belgrade", "nunit_test_Task_Party_Street_1", "nunit_test_taskpartyimg1.jpg");

        var taskCreate = new TaskCreate
        {
            Name = "nunit_test_task_1",
            Description = "This is nunit test task 1"
        };

        int initialTaskCount = await GetTaskCountAsync();

        // Act
        await _taskService.CreateTask(taskCreate, user.Id, party.Id);

        // Assert
        int finalTaskCount = await GetTaskCountAsync();
        Assert.That(finalTaskCount, Is.EqualTo(initialTaskCount + 1));

        var createdTask = await Context.Tasks
            .Where(t => t.Name == "nunit_test_task_1")
            .FirstOrDefaultAsync();

        Assert.That(createdTask, Is.Not.Null);
        Assert.That(createdTask!.Description, Is.EqualTo("This is nunit test task 1"));
        Assert.That(createdTask.User.Id, Is.EqualTo(user.Id));
        Assert.That(createdTask.Party.Id, Is.EqualTo(party.Id));
    }

    [Test]
    public async Task CreateTask_WithInvalidUserId_ShouldThrowException()
    {
        // Arrange
        var creator = await CreateTestUserAsync("nunit_test_task_creator_2", "nunit_test_taskcreator2@test.com", "nunit_test_passTaskCreator2", "nunit_test_avatarTaskCreator2.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_task_party_2", "Belgrade", "nunit_test_Task_Party_Street_2", "nunit_test_taskpartyimg2.jpg");

        var taskCreate = new TaskCreate
        {
            Name = "nunit_test_task_2",
            Description = "This is nunit test task 2"
        };

        int invalidUserId = 99999;

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _taskService.CreateTask(taskCreate, invalidUserId, party.Id));
        Assert.That(ex!.Message, Contains.Substring("User or Party not found"));
    }

    [Test]
    public async Task CreateTask_WithInvalidPartyId_ShouldThrowException()
    {
        // Arrange
        var user = await CreateTestUserAsync("nunit_test_task_user_2", "nunit_test_taskuser2@test.com", "nunit_test_passTaskUser2", "nunit_test_avatarTaskUser2.jpg");
        int invalidPartyId = 99999;

        var taskCreate = new TaskCreate
        {
            Name = "nunit_test_task_3",
            Description = "This is nunit test task 3"
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _taskService.CreateTask(taskCreate, user.Id, invalidPartyId));
        Assert.That(ex!.Message, Contains.Substring("User or Party not found"));
    }

    #endregion

    #region GetUserTasks Tests (READ)

    [Test]
    public async Task GetUserTasks_WithValidUserId_ShouldReturnUserTasks()
    {
        // Arrange
        var user = await CreateTestUserAsync("nunit_test_task_user_3", "nunit_test_taskuser3@test.com", "nunit_test_passTaskUser3", "nunit_test_avatarTaskUser3.jpg");
        var creator = await CreateTestUserAsync("nunit_test_task_creator_3", "nunit_test_taskcreator3@test.com", "nunit_test_passTaskCreator3", "nunit_test_avatarTaskCreator3.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_task_party_3", "Belgrade", "nunit_test_Task_Party_Street_3", "nunit_test_taskpartyimg3.jpg");
        var task1 = await CreateTestTaskAsync(user, party, "nunit_test_task_4", "Task 4 description");
        var task2 = await CreateTestTaskAsync(user, party, "nunit_test_task_5", "Task 5 description");

        // Act
        var userTasks = await _taskService.GetUserTasks(user.Id);

        // Assert
        Assert.That(userTasks, Is.Not.Null);
        Assert.That(userTasks.Count, Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public async Task GetUserTasks_WithInvalidUserId_ShouldThrowException()
    {
        // Arrange
        int invalidUserId = 99999;

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _taskService.GetUserTasks(invalidUserId));
        Assert.That(ex!.Message, Contains.Substring("User not found"));
    }

    [Test]
    public async Task GetUserTasks_WithUserWithoutTasks_ShouldReturnEmptyList()
    {
        // Arrange
        var user = await CreateTestUserAsync("nunit_test_task_user_4", "nunit_test_taskuser4@test.com", "nunit_test_passTaskUser4", "nunit_test_avatarTaskUser4.jpg");

        // Act
        var userTasks = await _taskService.GetUserTasks(user.Id);

        // Assert
        Assert.That(userTasks, Is.Not.Null);
        Assert.That(userTasks.Count, Is.EqualTo(0));
    }

    #endregion

    #region EditTask Tests (UPDATE)

    [Test]
    public async Task EditTask_WithValidData_ShouldUpdateTaskSuccessfully()
    {
        // Arrange
        var user = await CreateTestUserAsync("nunit_test_task_user_5", "nunit_test_taskuser5@test.com", "nunit_test_passTaskUser5", "nunit_test_avatarTaskUser5.jpg");
        var creator = await CreateTestUserAsync("nunit_test_task_creator_4", "nunit_test_taskcreator4@test.com", "nunit_test_passTaskCreator4", "nunit_test_avatarTaskCreator4.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_task_party_4", "Belgrade", "nunit_test_Task_Party_Street_4", "nunit_test_taskpartyimg4.jpg");
        var task = await CreateTestTaskAsync(user, party, "nunit_test_task_6", "Original description");

        var taskUpdate = new TaskUpdate
        {
            Name = "nunit_test_task_6_updated",
            Description = "Updated description for nunit test task 6"
        };

        // Act
        await _taskService.EditTask(taskUpdate, task.Id);

        // Assert
        var updatedTask = await TaskRepository.GetByIdAsync(task.Id);
        Assert.That(updatedTask, Is.Not.Null);
        Assert.That(updatedTask!.Name, Is.EqualTo("nunit_test_task_6_updated"));
        Assert.That(updatedTask.Description, Is.EqualTo("Updated description for nunit test task 6"));
    }

    [Test]
    public async Task EditTask_WithInvalidTaskId_ShouldThrowException()
    {
        // Arrange
        int invalidTaskId = 99999;
        var taskUpdate = new TaskUpdate
        {
            Name = "nunit_test_someTask",
            Description = "nunit_test_Some_description"
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _taskService.EditTask(taskUpdate, invalidTaskId));
        Assert.That(ex!.Message, Contains.Substring("Task not found"));
    }

    #endregion

    #region RemoveTask Tests (DELETE)

    [Test]
    public async Task RemoveTask_WithValidTaskId_ShouldDeleteTaskSuccessfully()
    {
        // Arrange
        var user = await CreateTestUserAsync("nunit_test_task_user_6", "nunit_test_taskuser6@test.com", "nunit_test_passTaskUser6", "nunit_test_avatarTaskUser6.jpg");
        var creator = await CreateTestUserAsync("nunit_test_task_creator_5", "nunit_test_taskcreator5@test.com", "nunit_test_passTaskCreator5", "nunit_test_avatarTaskCreator5.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_task_party_5", "Belgrade", "nunit_test_Task_Party_Street_5", "nunit_test_taskpartyimg5.jpg");
        var task = await CreateTestTaskAsync(user, party, "nunit_test_task_7", "Task to be deleted");
        int taskId = task.Id;

        // Act
        await _taskService.RemoveTask(taskId);

        // Assert
        var deletedTask = await TaskRepository.GetByIdAsync(taskId);
        Assert.That(deletedTask, Is.Null);
    }

    [Test]
    public async Task RemoveTask_WithInvalidTaskId_ShouldThrowException()
    {
        // Arrange
        int invalidTaskId = 99999;

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _taskService.RemoveTask(invalidTaskId));
        Assert.That(ex!.Message, Contains.Substring("Task not found"));
    }

    #endregion
}
