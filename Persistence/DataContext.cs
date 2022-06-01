using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityAtendee> ActivityAtendees { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ActivityAtendee>(k => k.HasKey(a => 
            new { a.AppUserId, a.ActivityId }));

        builder.Entity<ActivityAtendee>()
            .HasOne(u => u.AppUser)
            .WithMany(a => a.Activities)
            .HasForeignKey(aa => aa.AppUserId);
        
        builder.Entity<ActivityAtendee>()
            .HasOne(u => u.Activity)
            .WithMany(a => a.Atendees)
            .HasForeignKey(aa => aa.ActivityId);

        builder.Entity<Comment>()
            .HasOne(a => a.Activity)
            .WithMany(c => c.Comments)
            .OnDelete(DeleteBehavior.Cascade);
    }
}