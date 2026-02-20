using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestiranjeAPI.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Avatar { get; set; }
    public List<Party>? CreatedParties { get; set; }
    public List<PartyAttendance>? AttendingParties { get; set; }

    public User() { }

    public User(string username, string email, string password, string avatar)
    {
        Username = username;
        Email = email;
        Password = password;
        Avatar = avatar;
    }
}
