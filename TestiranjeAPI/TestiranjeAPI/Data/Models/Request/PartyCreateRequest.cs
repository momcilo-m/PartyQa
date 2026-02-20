namespace TestiranjeAPI.Models.Request;

public record PartyCreateRequest(string Name, string City, string Address, string Image)
{
    public PartyCreateRequest() : this("", "", "", "") { }

}