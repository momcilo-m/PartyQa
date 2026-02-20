using Microsoft.EntityFrameworkCore;
using TestiranjeAPI.Models;
using TestiranjeAPI.Repository;
using TestiranjeAPI.Repository.Interfaces;

namespace TestiranjeAPI.Component.Tests;

/// <summary>
/// Bazna klasa za sve komponentne testove. Inicijalizuje DbContext i omogućava pristup repositories.
/// </summary>
public abstract class BaseComponentTest
{
    protected PartyContext Context = null!;
    protected IUserRepository UserRepository = null!;
    protected IPartyRepository PartyRepository = null!;
    protected ITaskRepository TaskRepository = null!;

    private const string ConnectionString = "User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=PartyDatabase;Pooling=true;";

    [SetUp]
    public virtual void Setup()
    {
        var options = new DbContextOptionsBuilder<PartyContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        Context = new PartyContext(options);

        UserRepository = new UserRepository(Context);
        PartyRepository = new PartyRepository(Context);
        TaskRepository = new TaskRepository(Context);
    }

    [TearDown]
    public virtual async Task TearDown()
    {
        // Cleanup će se izvršiti u individualnim testovima kao deo Test Teardown metoda
        //await Context.DisposeAsync();
    }

    /// <summary>
    /// Kreira test korisnika sa predvidljivim podacima
    /// </summary>
    protected async Task<User> CreateTestUserAsync(string username = "testuser1", 
        string email = "test1@test.com", 
        string password = "password123", 
        string avatar = "avatar1.jpg")
    {
        var user = new User(username, email, password, avatar);
        await UserRepository.AddAsync(user);
        await UserRepository.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Briše test korisnika iz baze
    /// </summary>
    protected async Task DeleteTestUserAsync(User user)
    {
        UserRepository.Delete(user);
        await UserRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Kreira test zabavu sa predvidljivim podacima
    /// </summary>
    protected async Task<Party> CreateTestPartyAsync(User creator, 
        string name = "testparty1", 
        string city = "Belgrade", 
        string address = "Test Street 1", 
        string image = "party1.jpg")
    {
        var party = new Party(name, city, address, image)
        {
            Creator = creator
        };
        await PartyRepository.AddAsync(party);
        await PartyRepository.SaveChangesAsync();
        return party;
    }

    /// <summary>
    /// Briše test zabavu iz baze
    /// </summary>
    protected async Task DeleteTestPartyAsync(Party party)
    {
        PartyRepository.Delete(party);
        await PartyRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Kreira test zadatak sa predvidljivim podacima
    /// </summary>
    protected async Task<Models.Special.Task> CreateTestTaskAsync(User user, 
        Party party, 
        string name = "testtask1", 
        string description = "Test task description")
    {
        var task = new Models.Special.Task(name, description)
        {
            User = user,
            Party = party
        };
        await TaskRepository.AddAsync(task);
        await TaskRepository.SaveChangesAsync();
        return task;
    }

    /// <summary>
    /// Briše test zadatak iz baze
    /// </summary>
    protected async Task DeleteTestTaskAsync(Models.Special.Task task)
    {
        TaskRepository.Delete(task);
        await TaskRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Korisna metoda za čitanje broja redova iz baze za testiranje
    /// </summary>
    protected async Task<int> GetUserCountAsync()
    {
        return await Context.Users.CountAsync();
    }

    protected async Task<int> GetPartyCountAsync()
    {
        return await Context.Parties.CountAsync();
    }

    protected async Task<int> GetTaskCountAsync()
    {
        return await Context.Tasks.CountAsync();
    }
}
