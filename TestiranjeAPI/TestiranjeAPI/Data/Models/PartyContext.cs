using Microsoft.EntityFrameworkCore;

namespace TestiranjeAPI.Models;

public class PartyContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Party> Parties { get; set; }
    public DbSet<Special.Task> Tasks { get; set; }
    public DbSet<PartyAttendance> PartyAttendances { get; set; }
    public PartyContext(DbContextOptions<PartyContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }


}
