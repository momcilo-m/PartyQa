using TestiranjeAPI.Services.Interfaces;
using TestiranjeAPI.Models;
using TestiranjeAPI.Models.Request;
using TestiranjeAPI.Models.Response;
using TestiranjeAPI.Repository;
using TestiranjeAPI.Repository.Interfaces;

namespace TestiranjeAPI.Services;

public class PartyService : IPartyService
{

    private IUserRepository _userRepository;
    private IPartyRepository _partyRepository;

    public PartyService(IUserRepository userRepository, IPartyRepository partyRepository)
    {
        _userRepository = userRepository;
        _partyRepository = partyRepository;
    }

    public async Task<List<UserPartyResponse>> GetUserParty(int userId)
    {
        var existingUser = await _userRepository.GetByIdAsync(userId);

        if (existingUser == null) throw new Exception("User Not found");

        return await _partyRepository.GetUserCreatedPartiesAsync(userId);
    }

    public async Task<List<PartyCardResponse>?> GetAllParties()
    {
        return await _partyRepository.GetAllPartiesAsync();
    }

    public async Task<List<UserAttendingPartyResponse>?> GetUserAttendingParties(int userId)
    {
        return await _partyRepository.GetUserAttendingPartiesAsync(userId);
    }

    public async Task<List<PartyNameIdResponse>> GetUserCreatedPartiesNames(int userId)
    {
        return await _partyRepository.GetUserCreatedPartiesNamesAsync(userId);
    }

    public async Task CreateParty(PartyCreateRequest party, int userId)
    {
        var existingUser = await _userRepository.GetByIdAsync(userId);

        if (existingUser == null) throw new Exception("User not found");

        Party newParty = new Party(party.Name, party.City, party.Address, party.Image);
        newParty.Creator = existingUser;

        await _partyRepository.AddAsync(newParty);
        await _partyRepository.SaveChangesAsync();
    }

    public async Task AttendParty(int partyId, int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        var party = await _partyRepository.GetByIdAsync(partyId);

        if (party == null || user == null) throw new Exception("User or party not found");

        PartyAttendance partyAttendance = new PartyAttendance(user, party);

        await _partyRepository.AddPartyAttendanceAsync(partyAttendance);
        await _partyRepository.SaveChangesAsync();
    }

    public async Task CancelUserAttendance(int partyId, int userId)
    {
        var partyAttendanceToDelete = await _partyRepository.GetUserAttendanceAsync(partyId, userId);

        if (partyAttendanceToDelete == null) throw new Exception("User Attendance to party not found");

        _partyRepository.RemovePartyAttendance(partyAttendanceToDelete);
        await _partyRepository.SaveChangesAsync();
    }

    public async Task CancelUserParty(int partyId)
    {

        var partyToDelete = await _partyRepository.GetByIdAsync(partyId);

        if (partyToDelete == null) throw new Exception("Party not found");

        _partyRepository.Delete(partyToDelete);
        await _partyRepository.SaveChangesAsync();
    }

    public async Task EditParty(PartyUpdate partyUpdate, int partyId)
    {

        var partyToEdit = await _partyRepository.GetByIdAsync(partyId);

        if (partyToEdit == null) throw new Exception("Party not found");

        partyToEdit.Name = partyUpdate.Name;
        partyToEdit.City = partyUpdate.City;
        partyToEdit.Address = partyUpdate.Address;
        partyToEdit.Image = partyUpdate.Image;

        _partyRepository.Update(partyToEdit);
        await _partyRepository.SaveChangesAsync();

    }
}