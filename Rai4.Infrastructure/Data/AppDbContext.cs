using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Rai4.Domain.Models;

namespace Rai4.Infrastructure.Data;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IHttpContextAccessor? httpContextAccessor = null) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<BusStop> BusStops { get; set; }

    private int? CurrentUserId
    {
        get
        {
            var userIdString = httpContextAccessor?.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userIdString != null ? int.Parse(userIdString) : null;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
        });
        
        modelBuilder.Entity<BusStop>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.Name).IsRequired();
            entity.HasOne(e => e.User)
                  .WithMany(u => u.BusStops)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new {e.UserId, e.ZtmStopId}).IsUnique();
            
            entity.HasQueryFilter(bs => bs.UserId == CurrentUserId);
        });
    }
}