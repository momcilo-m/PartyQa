using TestiranjeAPI.Models;
using TestiranjeAPI.Models.Request;
using TestiranjeAPI.Models.Response;

namespace TestiranjeAPI.Services.Interfaces;

public interface IPartyService
{
    public Task<List<UserPartyResponse>> GetUserParty(int userId);
    public Task<List<PartyCardResponse>?> GetAllParties();
    public Task<List<UserAttendingPartyResponse>?> GetUserAttendingParties(int userId);
    public Task<List<PartyNameIdResponse>> GetUserCreatedPartiesNames(int userId);
    public Task CreateParty(PartyCreateRequest party, int userId);
    public Task AttendParty(int partyId, int userId);
    public Task CancelUserAttendance(int partyId, int userId);
    public Task CancelUserParty(int partyId);
    public Task EditParty(PartyUpdate partyUpdate, int partyId);
}