using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestiranjeAPI.Models;

public class PartyAttendance
{
    [Key]
    public int Id { get; set; }
    public User User { get; set; }
    public Party Party { get; set; }

    public PartyAttendance() { }

    public PartyAttendance(User user, Party party)
    {
        User = user;
        Party = party;
    }
}
