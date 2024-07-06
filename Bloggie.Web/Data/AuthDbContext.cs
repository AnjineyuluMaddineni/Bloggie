using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class AuthDbContext:IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            //Seeds Roles(User,Admin,SuperAdmin)

            var adminRoleId = "837dd1df-1f7f-433a-86ae-b22fd069e2bc";

            var superAdminRoleId = "fd1a71b6-7836-4155-a72b-5ca3cd7ba902";

            var userRoleId = "35028c0f-5061-4b37-85b5-75ee1f085f9e";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name= "Admin",
                    NormalizedName="Admin",
                    Id=adminRoleId,
                    ConcurrencyStamp=adminRoleId
                },
                 new IdentityRole
                 {
                    Name= "SuperAdmin",
                    NormalizedName="SuperAdmin",
                    Id=superAdminRoleId,
                    ConcurrencyStamp=superAdminRoleId
                 },

                 new IdentityRole
                 {
                    Name= "User",
                    NormalizedName="User",
                    Id=userRoleId,
                    ConcurrencyStamp=userRoleId
                 },
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //Seed SuperAdminUser
            var superAdminId = "f542b128-34a4-4146-81fc-6b9b8b2821de";

            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add All roles to SuperAdminUser

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId=adminRoleId,
                    UserId=superAdminId,

                },
                new IdentityUserRole<string>
                {
                    RoleId=superAdminRoleId,
                    UserId=superAdminId,

                },
                new IdentityUserRole<string>
                {
                    RoleId=userRoleId,
                    UserId=superAdminId,

                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
