using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data;

public class NZWalksAuthDbContext(DbContextOptions options) : IdentityDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var readerRoleId = "afa35c1f-f06d-4001-82d8-aff1ec332516";
        var writerRoleId = "81513552-a269-4ebd-9491-4939163f785f";

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = readerRoleId,
                ConcurrencyStamp = readerRoleId,
                Name = "Reader",
                NormalizedName = "READER"
            },
            new IdentityRole
            {
                Id = writerRoleId,
                ConcurrencyStamp = writerRoleId,
                Name = "Writer",
                NormalizedName = "WRITER",
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}