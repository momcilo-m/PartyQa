using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestiranjeAPI.Models;

//Dodati mozda vreme kasnije
public class Party
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string Image { get; set; }

    public User? Creator { get; set; }
    public List<PartyAttendance>? Attendees { get; set; }

    public Party() { }
    public Party(string name, string city, string address, string image)
    {
        Name = name;
        City = city;
        Address = address;
        Image = image;
    }
}
