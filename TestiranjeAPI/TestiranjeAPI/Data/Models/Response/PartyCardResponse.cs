namespace TestiranjeAPI.Models.Response;

public record PartyCardResponse(
    int Id,
    string Name,
    string City,
    string Address,
    string Image,
    string Creator,
    int CreatorId);
