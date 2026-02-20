using TestiranjeAPI.Models;
using TestiranjeAPI.Models.Response;
using Task = System.Threading.Tasks.Task;

namespace TestiranjeAPI.Repository.Interfaces;

public interface IPartyRepository : IRepository<Party>
{
    Task<List<UserPartyResponse>> GetUserCreatedPartiesAsync(int userId);
    public Task<List<PartyCardResponse>> GetAllPartiesAsync();
    Task<List<UserAttendingPartyResponse>> GetUserAttendingPartiesAsync(int userId);
    Task<List<PartyNameIdResponse>> GetUserCreatedPartiesNamesAsync(int userId);
    Task<PartyAttendance?> GetUserAttendanceAsync(int partyId, int userId);
    Task AddPartyAttendanceAsync(PartyAttendance partyAttendance);
    void RemovePartyAttendance(PartyAttendance partyAttendance);
    Task SaveChangesAsync();
}
