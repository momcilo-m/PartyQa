using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestiranjeAPI.Models.Special;

public class Task
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public User User { get; set; }
    public Party Party { get; set; }

    public Task() { }

    public Task(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
