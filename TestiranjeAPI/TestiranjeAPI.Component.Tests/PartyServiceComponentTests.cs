using TestiranjeAPI.Models;
using TestiranjeAPI.Models.Request;
using TestiranjeAPI.Services;

namespace TestiranjeAPI.Component.Tests;

[TestFixture]
public class PartyServiceComponentTests : BaseComponentTest
{
    private PartyService _partyService = null!;
    private const string TestPrefix = "nunit_test_";

    public override void Setup()
    {
        base.Setup();
        _partyService = new PartyService(UserRepository, PartyRepository);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
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

        await PartyRepository.SaveChangesAsync();
    }

    #region CreateParty Tests (CREATE)

    [Test]
    public async Task CreateParty_WithValidData_ShouldCreatePartySuccessfully()
    {
        // Arrange
        var creator = await CreateTestUserAsync("nunit_test_party_creator_1", "nunit_test_partycreator1@test.com", "nunit_test_passCreator1", "nunit_test_avatar1.jpg");
        
        var partyRequest = new PartyCreateRequest("nunit_test_party_1", "Belgrade", "nunit_test_Party_Street_1", "nunit_test_partyimg1.jpg");
        int initialPartyCount = await GetPartyCountAsync();

        // Act
        await _partyService.CreateParty(partyRequest, creator.Id);

        // Assert
        int finalPartyCount = await GetPartyCountAsync();
        Assert.That(finalPartyCount, Is.EqualTo(initialPartyCount + 1));

        var createdParty = await Context.Parties
            .Where(p => p.Name == "nunit_test_party_1")
            .FirstOrDefaultAsync();
        
        Assert.That(createdParty, Is.Not.Null);
        Assert.That(createdParty!.City, Is.EqualTo("Belgrade"));
        Assert.That(createdParty.Address, Is.EqualTo("nunit_test_Party_Street_1"));
        Assert.That(createdParty.Creator?.Id, Is.EqualTo(creator.Id));
    }

    [Test]
    public async Task CreateParty_WithInvalidUserId_ShouldThrowException()
    {
        // Arrange
        int invalidUserId = 99999;
        var partyRequest = new PartyCreateRequest("nunit_test_party_2", "Belgrade", "nunit_test_Party_Street_2", "nunit_test_partyimg2.jpg");

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _partyService.CreateParty(partyRequest, invalidUserId));
        Assert.That(ex!.Message, Contains.Substring("User not found"));
    }

    #endregion

    #region GetUserParty Tests (READ)

    [Test]
    public async Task GetUserParty_WithValidUserId_ShouldReturnUserCreatedParties()
    {
        // Arrange
        var creator = await CreateTestUserAsync("nunit_test_party_creator_2", "nunit_test_partycreator2@test.com", "nunit_test_passCreator2", "nunit_test_avatar2.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_party_3", "Niš", "nunit_test_Party_Street_3", "nunit_test_partyimg3.jpg");

        // Act
        var userParties = await _partyService.GetUserParty(creator.Id);

        // Assert
        Assert.That(userParties, Is.Not.Null);
        Assert.That(userParties.Count, Is.GreaterThanOrEqualTo(1));
        var foundParty = userParties.FirstOrDefault(p => p.Id == party.Id);
        Assert.That(foundParty, Is.Not.Null);
        Assert.That(foundParty!.Name, Is.EqualTo("nunit_test_party_3"));
    }

    [Test]
    public async Task GetUserParty_WithInvalidUserId_ShouldThrowException()
    {
        // Arrange
        int invalidUserId = 99999;

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _partyService.GetUserParty(invalidUserId));
        Assert.That(ex!.Message, Contains.Substring("User Not found"));
    }

    #endregion

    #region GetAllParties Tests (READ)

    [Test]
    public async Task GetAllParties_ShouldReturnAllAvailableParties()
    {
        // Arrange
        var creator = await CreateTestUserAsync("nunit_test_party_creator_3", "nunit_test_partycreator3@test.com", "nunit_test_passCreator3", "nunit_test_avatar3.jpg");
        var party1 = await CreateTestPartyAsync(creator, "nunit_test_party_4", "Belgrade", "nunit_test_Party_Street_4", "nunit_test_partyimg4.jpg");
        var party2 = await CreateTestPartyAsync(creator, "nunit_test_party_5", "Niš", "nunit_test_Party_Street_5", "nunit_test_partyimg5.jpg");

        // Act
        var allParties = await _partyService.GetAllParties();

        // Assert
        Assert.That(allParties, Is.Not.Null);
        Assert.That(allParties!.Count, Is.GreaterThanOrEqualTo(2));
        var foundParty1 = allParties.FirstOrDefault(p => p.Id == party1.Id);
        var foundParty2 = allParties.FirstOrDefault(p => p.Id == party2.Id);
        Assert.That(foundParty1, Is.Not.Null);
        Assert.That(foundParty2, Is.Not.Null);
    }

    [Test]
    public async Task GetAllParties_WhenNoPartiesExist_ShouldReturnEmptyList()
    {
        // Act
        var allParties = await _partyService.GetAllParties();

        // Assert
        Assert.That(allParties, Is.Not.Null);
        Assert.That(allParties!.Count, Is.GreaterThanOrEqualTo(0));
    }

    #endregion

    #region GetUserAttendingParties Tests (READ)

    [Test]
    public async Task GetUserAttendingParties_WithAttendingUser_ShouldReturnAttendingParties()
    {
        // Arrange
        var creator = await CreateTestUserAsync("nunit_test_party_creator_4", "nunit_test_partycreator4@test.com", "nunit_test_passCreator4", "nunit_test_avatar4.jpg");
        var attendee = await CreateTestUserAsync("nunit_test_attendee_1", "nunit_test_attendee1@test.com", "nunit_test_passAttendee1", "nunit_test_avatarAttendee1.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_party_6", "Belgrade", "nunit_test_Party_Street_6", "nunit_test_partyimg6.jpg");

        var partyAttendance = new PartyAttendance(attendee, party);
        await PartyRepository.AddPartyAttendanceAsync(partyAttendance);
        await PartyRepository.SaveChangesAsync();

        // Act
        var attendingParties = await _partyService.GetUserAttendingParties(attendee.Id);

        // Assert
        Assert.That(attendingParties, Is.Not.Null);
        Assert.That(attendingParties!.Count, Is.GreaterThanOrEqualTo(1));
        var foundParty = attendingParties.FirstOrDefault(p => p.Id == party.Id);
        Assert.That(foundParty, Is.Not.Null);
    }

    [Test]
    public async Task GetUserAttendingParties_WithNoAttendingParties_ShouldReturnEmptyList()
    {
        // Arrange
        var user = await CreateTestUserAsync("nunit_test_attendee_2", "nunit_test_attendee2@test.com", "nunit_test_passAttendee2", "nunit_test_avatarAttendee2.jpg");

        // Act
        var attendingParties = await _partyService.GetUserAttendingParties(user.Id);

        // Assert
        Assert.That(attendingParties, Is.Not.Null);
        Assert.That(attendingParties!.Count, Is.EqualTo(0));
    }

    #endregion

    #region GetUserCreatedPartiesNames Tests (READ)

    [Test]
    public async Task GetUserCreatedPartiesNames_WithValidUserId_ShouldReturnPartyNames()
    {
        // Arrange
        var creator = await CreateTestUserAsync("nunit_test_party_creator_5", "nunit_test_partycreator5@test.com", "nunit_test_passCreator5", "nunit_test_avatar5.jpg");
        var party1 = await CreateTestPartyAsync(creator, "nunit_test_party_7", "Belgrade", "nunit_test_Party_Street_7", "nunit_test_partyimg7.jpg");
        var party2 = await CreateTestPartyAsync(creator, "nunit_test_party_8", "Niš", "nunit_test_Party_Street_8", "nunit_test_partyimg8.jpg");

        // Act
        var partyNames = await _partyService.GetUserCreatedPartiesNames(creator.Id);

        // Assert
        Assert.That(partyNames, Is.Not.Null);
        Assert.That(partyNames.Count, Is.GreaterThanOrEqualTo(2));
        var foundParty1 = partyNames.FirstOrDefault(p => p.Id == party1.Id);
        var foundParty2 = partyNames.FirstOrDefault(p => p.Id == party2.Id);
        Assert.That(foundParty1, Is.Not.Null);
        Assert.That(foundParty1!.Name, Is.EqualTo("nunit_test_party_7"));
        Assert.That(foundParty2, Is.Not.Null);
        Assert.That(foundParty2!.Name, Is.EqualTo("nunit_test_party_8"));
    }

    [Test]
    public async Task GetUserCreatedPartiesNames_WithUserWithoutParties_ShouldReturnEmptyList()
    {
        // Arrange
        var user = await CreateTestUserAsync("nunit_test_party_creator_6", "nunit_test_partycreator6@test.com", "nunit_test_passCreator6", "nunit_test_avatar6.jpg");

        // Act
        var partyNames = await _partyService.GetUserCreatedPartiesNames(user.Id);

        // Assert
        Assert.That(partyNames, Is.Not.Null);
        Assert.That(partyNames.Count, Is.EqualTo(0));
    }

    #endregion

    #region AttendParty Tests (CREATE)

    [Test]
    public async Task AttendParty_WithValidPartyAndUserId_ShouldCreateAttendance()
    {
        // Arrange
        var creator = await CreateTestUserAsync("nunit_test_party_creator_7", "nunit_test_partycreator7@test.com", "nunit_test_passCreator7", "nunit_test_avatar7.jpg");
        var attendee = await CreateTestUserAsync("nunit_test_attendee_3", "nunit_test_attendee3@test.com", "nunit_test_passAttendee3", "nunit_test_avatarAttendee3.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_party_9", "Belgrade", "nunit_test_Party_Street_9", "nunit_test_partyimg9.jpg");

        // Act
        await _partyService.AttendParty(party.Id, attendee.Id);

        // Assert
        var attendance = await PartyRepository.GetUserAttendanceAsync(party.Id, attendee.Id);
        Assert.That(attendance, Is.Not.Null);
        Assert.That(attendance!.User.Id, Is.EqualTo(attendee.Id));
        Assert.That(attendance.Party.Id, Is.EqualTo(party.Id));
    }

    [Test]
    public async Task AttendParty_WithInvalidPartyId_ShouldThrowException()
    {
        // Arrange
        var attendee = await CreateTestUserAsync("nunit_test_attendee_4", "nunit_test_attendee4@test.com", "nunit_test_passAttendee4", "nunit_test_avatarAttendee4.jpg");
        int invalidPartyId = 99999;

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _partyService.AttendParty(invalidPartyId, attendee.Id));
        Assert.That(ex!.Message, Contains.Substring("User or party not found"));
    }

    #endregion

    #region EditParty Tests (UPDATE)

    [Test]
    public async Task EditParty_WithValidData_ShouldUpdatePartySuccessfully()
    {
        // Arrange
        var creator = await CreateTestUserAsync("nunit_test_party_creator_8", "nunit_test_partycreator8@test.com", "nunit_test_passCreator8", "nunit_test_avatar8.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_party_10", "Belgrade", "nunit_test_Party_Street_10", "nunit_test_partyimg10.jpg");

        var partyUpdate = new PartyUpdate
        {
            Name = "nunit_test_party_10_updated",
            City = "Niš",
            Address = "nunit_test_Updated_Party_Street_10",
            Image = "nunit_test_updatedpartyimg10.jpg"
        };

        // Act
        await _partyService.EditParty(partyUpdate, party.Id);

        // Assert
        var updatedParty = await PartyRepository.GetByIdAsync(party.Id);
        Assert.That(updatedParty, Is.Not.Null);
        Assert.That(updatedParty!.Name, Is.EqualTo("nunit_test_party_10_updated"));
        Assert.That(updatedParty.City, Is.EqualTo("Niš"));
        Assert.That(updatedParty.Address, Is.EqualTo("nunit_test_Updated_Party_Street_10"));
        Assert.That(updatedParty.Image, Is.EqualTo("nunit_test_updatedpartyimg10.jpg"));
    }

    [Test]
    public async Task EditParty_WithInvalidPartyId_ShouldThrowException()
    {
        // Arrange
        int invalidPartyId = 99999;
        var partyUpdate = new PartyUpdate
        {
            Name = "nunit_test_someParty",
            City = "nunit_test_someCity",
            Address = "nunit_test_someAddress",
            Image = "nunit_test_someImage"
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _partyService.EditParty(partyUpdate, invalidPartyId));
        Assert.That(ex!.Message, Contains.Substring("Party not found"));
    }

    #endregion

    #region CancelUserAttendance Tests (DELETE)

    [Test]
    public async Task CancelUserAttendance_WithValidAttendance_ShouldRemoveAttendance()
    {
        // Arrange
        var creator = await CreateTestUserAsync("nunit_test_party_creator_9", "nunit_test_partycreator9@test.com", "nunit_test_passCreator9", "nunit_test_avatar9.jpg");
        var attendee = await CreateTestUserAsync("nunit_test_attendee_5", "nunit_test_attendee5@test.com", "nunit_test_passAttendee5", "nunit_test_avatarAttendee5.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_party_11", "Belgrade", "nunit_test_Party_Street_11", "nunit_test_partyimg11.jpg");

        var partyAttendance = new PartyAttendance(attendee, party);
        await PartyRepository.AddPartyAttendanceAsync(partyAttendance);
        await PartyRepository.SaveChangesAsync();

        // Act
        await _partyService.CancelUserAttendance(party.Id, attendee.Id);

        // Assert
        var deletedAttendance = await PartyRepository.GetUserAttendanceAsync(party.Id, attendee.Id);
        Assert.That(deletedAttendance, Is.Null);
    }

    [Test]
    public async Task CancelUserAttendance_WithNonExistentAttendance_ShouldThrowException()
    {
        // Arrange
        var creator = await CreateTestUserAsync("nunit_test_party_creator_10", "nunit_test_partycreator10@test.com", "nunit_test_passCreator10", "nunit_test_avatar10.jpg");
        var attendee = await CreateTestUserAsync("nunit_test_attendee_6", "nunit_test_attendee6@test.com", "nunit_test_passAttendee6", "nunit_test_avatarAttendee6.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_party_12", "Belgrade", "nunit_test_Party_Street_12", "nunit_test_partyimg12.jpg");

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _partyService.CancelUserAttendance(party.Id, attendee.Id));
        Assert.That(ex!.Message, Contains.Substring("User Attendance to party not found"));
    }

    #endregion

    #region CancelUserParty Tests (DELETE)

    [Test]
    public async Task CancelUserParty_WithValidPartyId_ShouldDeletePartySuccessfully()
    {
        // Arrange
        var creator = await CreateTestUserAsync("nunit_test_party_creator_11", "nunit_test_partycreator11@test.com", "nunit_test_passCreator11", "nunit_test_avatar11.jpg");
        var party = await CreateTestPartyAsync(creator, "nunit_test_party_13", "Belgrade", "nunit_test_Party_Street_13", "nunit_test_partyimg13.jpg");
        int partyId = party.Id;

        // Act
        await _partyService.CancelUserParty(partyId);

        // Assert
        var deletedParty = await PartyRepository.GetByIdAsync(partyId);
        Assert.That(deletedParty, Is.Null);
    }

    [Test]
    public async Task CancelUserParty_WithInvalidPartyId_ShouldThrowException()
    {
        // Arrange
        int invalidPartyId = 99999;

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _partyService.CancelUserParty(invalidPartyId));
        Assert.That(ex!.Message, Contains.Substring("Party not found"));
    }

    #endregion
}
