using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TicketEase.Domain.Entities;


namespace TicketEase.Common.Utilities
{
    public class Seeder
    {
        private static string adminRoleId;
        private static string superAdminRoleId;
        private static string userRoleId;

        public static void SeedData(ModelBuilder builder)
        {
            SeedRoles(builder);
            SeedSuperAdminUser(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            var roles = new List<IdentityRole>
        {
            new IdentityRole
            {   Id = Guid.NewGuid().ToString(),
                Name = "Manager",
                NormalizedName = "Manager"
            },
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SuperAdmin",
                NormalizedName = "SuperAdmin"
            },
            new IdentityRole
            {

                Id = Guid.NewGuid().ToString(),
                Name = "User",
                NormalizedName = "User"
            }
        };

            builder.Entity<IdentityRole>().HasData(roles);

            // Store the role IDs for later use
            adminRoleId = roles.First(r => r.NormalizedName == "Manager").Id;
            superAdminRoleId = roles.First(r => r.NormalizedName == "SuperAdmin").Id;
            userRoleId = roles.First(r => r.NormalizedName == "User").Id;
        }

        private static void SeedSuperAdminUser(ModelBuilder builder)
        {
            var superAdminId = Guid.NewGuid().ToString();
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@library.com",
                Email = "superadmin@library.com",
                NormalizedEmail = "superadmin@library.com".ToUpper(),
                NormalizedUserName = "superadmin@library.com".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "SuperAdmin@123");
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // Use the stored role IDs
            var superAdminRoles = new List<IdentityUserRole<string>>
        {
            new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = superAdminId
            },
            new IdentityUserRole<string>
            {
                RoleId = superAdminRoleId,
                UserId = superAdminId
            },
            new IdentityUserRole<string>
            {
                RoleId = userRoleId,
                UserId = superAdminId
            }
        };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
