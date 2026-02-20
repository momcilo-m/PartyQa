namespace TestiranjeAPI.Models.Response;

public record UserAttendingPartyResponse(
    int Id,
    string Name,
    string City,
    string Address,
    string Image);
