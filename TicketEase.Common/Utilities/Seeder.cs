
using Microsoft.AspNetCore.Identity;
using TicketEase.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace TicketEase.Common.Utilities
{
	public class Seeder
	{
		public static void SeedRolesAndSuperAdmin(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

			// Seed roles
			if (!roleManager.RoleExistsAsync("SuperAdmin").Result)
			{
				var role = new IdentityRole("SuperAdmin");
				roleManager.CreateAsync(role).Wait();
			}

			if (!roleManager.RoleExistsAsync("Manager").Result)
			{
				var role = new IdentityRole("Manager");
				roleManager.CreateAsync(role).Wait();
			}

			if (!roleManager.RoleExistsAsync("User").Result)
			{
				var role = new IdentityRole("User");
				roleManager.CreateAsync(role).Wait();
			}

			// Seed users with roles
			if (userManager.FindByNameAsync("Admin").Result == null)
			{
				var user = new AppUser
				{
					UserName = "Admin",
					Email = "admin@ticketease.com",
					EmailConfirmed = true,
					FirstName = "Admin",
					IsActive = true,
					CreatedAt = DateTime.UtcNow
				};

				var result = userManager.CreateAsync(user, "Password@123").Result;

				if (result.Succeeded)
				{
					userManager.AddToRoleAsync(user, "SuperAdmin").Wait();
				}
			}
		}

	}
}
