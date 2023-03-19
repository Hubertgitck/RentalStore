using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentalCompany.Infrastructure.Data;
using Constants = RentalCompany.Utility.Constants;

namespace RentalCompany.Infrastructure.DbInitializer;

public class DbInitializer : IDbInitializer
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _dbContext;
    private const string initialAdminEmail = "admin@rental.com";
    private const string initialAdminPassword = "Admin123*";

    public DbInitializer(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext applicationDbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = applicationDbContext;
    }
    public async Task Initialize()
    {
        if (_dbContext.Database.GetPendingMigrations().Any())
        {
            _dbContext.Database.Migrate();
        }

        if (!await _roleManager.RoleExistsAsync(Constants.RoleAdmin))
        {
            await CreateRoles();
            await CreateAdminUser();

            ApplicationUser user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Email == initialAdminEmail);

            await _userManager.AddToRoleAsync(user, Constants.RoleAdmin);
        }
        return;
    }

    private async Task CreateRoles()
    {
        await _roleManager.CreateAsync(new IdentityRole(Constants.RoleAdmin));
        await _roleManager.CreateAsync(new IdentityRole(Constants.RoleEmployee));
        await _roleManager.CreateAsync(new IdentityRole(Constants.RoleCustomer));
    }

    private async Task CreateAdminUser()
    {
        await _userManager.CreateAsync(new ApplicationUser
        {
            UserName = initialAdminEmail,
            Email = initialAdminEmail,
            Name = "Admin admin",
            PhoneNumber = "1234567890",
            StreetAddress = "Street Address",
            State = "IL",
            PostalCode = "1234567890",
            City = "City"
        }, initialAdminPassword);
    }
}
