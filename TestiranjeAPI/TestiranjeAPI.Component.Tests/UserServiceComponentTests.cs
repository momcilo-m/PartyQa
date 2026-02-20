using AutoMapper;
using TestiranjeAPI.Models;
using TestiranjeAPI.Services;
using Backend.Application.Mappings;

namespace TestiranjeAPI.Component.Tests;

[TestFixture]
public class UserServiceComponentTests : BaseComponentTest
{
    private UserService _userService = null!;
    private IMapper _mapper = null!;
    private const string TestPrefix = "nunit_test_";

    public override void Setup()
    {
        base.Setup();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        _mapper = configuration.CreateMapper();
        _userService = new UserService(UserRepository, _mapper);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        var testUsers = await Context.Users
            .Where(u => u.Username.StartsWith(TestPrefix))
            .ToListAsync();

        foreach (var user in testUsers)
        {
            Console.WriteLine($"Deleting test user: {user.Username} (ID: {user.Id})");
            UserRepository.Delete(user);
        }

        await UserRepository.SaveChangesAsync();
    }

    #region Register Tests (CREATE)

    [Test]
    public async Task Register_WithValidData_ShouldCreateUserSuccessfully()
    {
        // Arrange
        var userRegister = new UserRegister
        {
            Username = "nunit_test_user_register_1",
            Email = "nunit_test_register1@test.com",
            Password = "testPassword123",
            Avatar = "nunit_test_avatar1.jpg"
        };

        int initialUserCount = await GetUserCountAsync();

        // Act
        var result = await _userService.Register(userRegister);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("nunit_test_user_register_1"));
        Assert.That(result.Email, Is.EqualTo("nunit_test_register1@test.com"));
        Assert.That(await GetUserCountAsync(), Is.EqualTo(initialUserCount + 1));
    }

    [Test]
    public async Task Register_WithDuplicateUsername_ShouldThrowException()
    {
        // Arrange
        var user1 = await CreateTestUserAsync("nunit_test_user_duplicate_1", "nunit_test_email1@test.com", "pass1", "nunit_test_avatar1.jpg");

        var userRegister = new UserRegister
        {
            Username = "nunit_test_user_duplicate_1",
            Email = "nunit_test_differentemail@test.com",
            Password = "password",
            Avatar = "nunit_test_avatar.jpg"
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _userService.Register(userRegister));
        Assert.That(ex!.Message, Contains.Substring("User exist"));
    }

    #endregion

    #region Login Tests (READ)

    [Test]
    public async Task Login_WithValidCredentials_ShouldReturnUserId()
    {
        // Arrange
        var testUser = await CreateTestUserAsync("nunit_test_user_login_1", "nunit_test_login1@test.com", "loginPass1", "nunit_test_avatar1.jpg");

        var userLogin = new UserLogin
        {
            Username = "nunit_test_user_login_1",
            Password = "loginPass1"
        };

        // Act
        int userId = await _userService.Login(userLogin);

        // Assert
        Assert.That(userId, Is.EqualTo(testUser.Id));
    }

    [Test]
    public async Task Login_WithInvalidCredentials_ShouldThrowException()
    {
        // Arrange
        var userLogin = new UserLogin
        {
            Username = "nunit_test_nonexistent_user_123",
            Password = "wrongPassword"
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _userService.Login(userLogin));
        Assert.That(ex!.Message, Contains.Substring("User not found"));
    }

    #endregion

    #region GetUserInfo Tests (READ)

    [Test]
    public async Task GetUserInfo_WithValidUserId_ShouldReturnUserInfo()
    {
        // Arrange
        var testUser = await CreateTestUserAsync("nunit_test_user_info_1", "nunit_test_info1@test.com", "infoPass1", "nunit_test_infoavatar1.jpg");

        // Act
        var userInfo = await _userService.GetUserInfo(testUser.Id);

        // Assert
        Assert.That(userInfo, Is.Not.Null);
        Assert.That(userInfo.Username, Is.EqualTo("nunit_test_user_info_1"));
        Assert.That(userInfo.Email, Is.EqualTo("nunit_test_info1@test.com"));
        Assert.That(userInfo.Password, Is.EqualTo("infoPass1"));
        Assert.That(userInfo.Avatar, Is.EqualTo("nunit_test_infoavatar1.jpg"));
    }

    [Test]
    public async Task GetUserInfo_WithInvalidUserId_ShouldThrowException()
    {
        // Arrange
        int invalidUserId = 99999;

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _userService.GetUserInfo(invalidUserId));
        Assert.That(ex!.Message, Contains.Substring("User not found"));
    }

    #endregion

    #region UpdateUser Tests (UPDATE)

    [Test]
    public async Task UpdateUser_WithValidData_ShouldUpdateUserSuccessfully()
    {
        // Arrange
        var testUser = await CreateTestUserAsync("nunit_test_user_update_1", "nunit_test_update1@test.com", "updatePass1", "nunit_test_updateavatar1.jpg");

        var userUpdate = new UserUpdate
        {
            Username = "nunit_test_user_update_1",
            Email = "nunit_test_updated1@test.com",
            Password = "newPassword123",
            Avatar = "nunit_test_newavatarUpdate1.jpg"
        };

        // Act
        await _userService.UpdateUser(testUser.Id, userUpdate);
        await UserRepository.SaveChangesAsync();

        // Assert
        var updatedUser = await UserRepository.GetByIdAsync(testUser.Id);
        Assert.That(updatedUser, Is.Not.Null);
        Assert.That(updatedUser!.Username, Is.EqualTo("nunit_test_user_update_1"));
        Assert.That(updatedUser.Email, Is.EqualTo("nunit_test_updated1@test.com"));
        Assert.That(updatedUser.Password, Is.EqualTo("newPassword123"));
        Assert.That(updatedUser.Avatar, Is.EqualTo("nunit_test_newavatarUpdate1.jpg"));
    }

    [Test]
    public async Task UpdateUser_WithInvalidUserId_ShouldThrowException()
    {
        // Arrange
        int invalidUserId = 99999;
        var userUpdate = new UserUpdate
        {
            Username = "nunit_test_someuser",
            Email = "nunit_test_someemail@test.com",
            Password = "nunit_test_somepass",
            Avatar = "nunit_test_someavatar.jpg"
        };

        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _userService.UpdateUser(invalidUserId, userUpdate));
    }

    #endregion
}
