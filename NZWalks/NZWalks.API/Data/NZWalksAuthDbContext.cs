using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext // inherit from Microsoft.AspNetCore.Identity.EntityFrameworkCore
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options) // used when we inject the DbContext in program.cs
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "740a487b-d88f-4976-9776-6ba2441c54fa";
            var writerRoleId = "cb8efc55-ed26-4310-87ad-ae27becf8b79";

            var roles = new List<IdentityRole> // IdentityRole from Microsoft.AspNetCore.Identity
            {
                new IdentityRole
                {
                    Id=readerRoleId,
                    ConcurrencyStamp=readerRoleId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id=writerRoleId,
                    ConcurrencyStamp=writerRoleId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
            // entityframeworkcore migration will add/seed this roles data in database
        }
    }
}
