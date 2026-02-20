using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
//using TestiranjeAPI.IRepository;
using TestiranjeAPI.Models;
using TestiranjeAPI.Models.Response;
using TestiranjeAPI.Repository.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TestiranjeAPI.Repository;

public class PartyRepository : Repository<Party>, IPartyRepository
{
    public PartyRepository(PartyContext context) : base(context) { }

    public async Task<List<UserPartyResponse>> GetUserCreatedPartiesAsync(int userId)
    {
        var parties = await _context.Parties
            .Include(p => p.Creator)
            .Where(p => p.Creator!.Id == userId)
            .Select(p => new UserPartyResponse(p.Id, p.Name, p.City, p.Address, p.Image))
            .ToListAsync();

        return parties;
    }
    public async Task<List<PartyCardResponse>> GetAllPartiesAsync()
    {
        var parties = await _context.Parties
            .Include(p => p.Creator)
            .Select(p => new PartyCardResponse(p.Id, p.Name, p.City, p.Address, p.Image, p.Creator!.Username, p.Creator.Id))
            .ToListAsync();

        return parties;
    }
    public async Task<List<UserAttendingPartyResponse>> GetUserAttendingPartiesAsync(int userId)
    {
        var parties = await _context.PartyAttendances
            .Include(pa => pa.User)
            .Include(pa => pa.Party)
            .Where(pa => pa.User.Id == userId)
            .Select(pa => new UserAttendingPartyResponse(pa.Party.Id, pa.Party.Name, pa.Party.City, pa.Party.Address, pa.Party.Image))
            .ToListAsync();

        return parties;
    }
    public async Task<List<PartyNameIdResponse>> GetUserCreatedPartiesNamesAsync(int userId)
    {
        var parties = await _context.Parties
            .Include(p => p.Creator)
            .Where(p => p.Creator!.Id == userId)
            .Select(p => new PartyNameIdResponse(p.Id, p.Name))
            .ToListAsync();

        return parties;
    }

    public async Task<PartyAttendance?> GetUserAttendanceAsync(int partyId, int userId)
    {
        var partyAttendance = await _context.PartyAttendances
            .Include(pa => pa.User)
            .Include(pa => pa.Party)
            .Where(pa => pa.Party.Id == partyId && pa.User.Id == userId)
            .FirstOrDefaultAsync();

        return partyAttendance;
    }

    public async Task AddPartyAttendanceAsync(PartyAttendance partyAttendance)
    {
        await _context.PartyAttendances.AddAsync(partyAttendance);
        return;
    }
    public void RemovePartyAttendance(PartyAttendance partyAttendance)
    {
        _context.Remove(partyAttendance);
        return;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
        return;
    }
}
