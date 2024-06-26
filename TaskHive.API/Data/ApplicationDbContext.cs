using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskHive.API.Models;
using TaskHive.API.Models.Account;

namespace TaskHive.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User, Role, string>(options)
{
    public DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>()
            .HasData(
                new Role()
                {
                    Id = "a1e38c3c-01ff-4565-a000-1383a13dc8dc",
                    Name = nameof(RoleNames.Admin),
                    NormalizedName = nameof(RoleNames.Admin).ToUpper(),
                }, new Role()
                {
                    Id = "fc132acc-32cf-4776-a82e-60daae619d83",
                    Name = nameof(RoleNames.User),
                    NormalizedName = nameof(RoleNames.User).ToUpper(),
                }
            );
    }
}