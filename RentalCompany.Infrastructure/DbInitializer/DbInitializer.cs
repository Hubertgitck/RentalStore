using System.Collections.Generic;
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
    private const string initialCustomerEmail = "customer@domain.com";
    private const string initialCustomerPassword = "Test123*";

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
            await CreateCustomerUser();

            ApplicationUser user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Email == initialAdminEmail);
            ApplicationUser customer = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Email == initialCustomerEmail);

            await _userManager.AddToRoleAsync(user, Constants.RoleAdmin);
            await _userManager.AddToRoleAsync(customer, Constants.RoleCustomer);
        }
        if (!_dbContext.RentalStores.ToList().Any() && !_dbContext.Cars.ToList().Any() && !_dbContext.ContactDatas.ToList().Any())
        {
            SeedTestData();
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
            Name = "John Admin",
            PhoneNumber = "+00 1234567890",
            StreetAddress = "Test Address 22",
            State = "AB",
            PostalCode = "123",
            City = "City"
        }, initialAdminPassword);
    }

    private async Task CreateCustomerUser()
    {
        await _userManager.CreateAsync(new ApplicationUser
        {
            UserName = initialCustomerEmail,
            Email = initialCustomerEmail,
            Name = "John Smith",
            PhoneNumber = "+11 1234567890",
            StreetAddress = "Test Address 33",
            State = "CD",
            PostalCode = "456",
            City = "City",
        }, initialCustomerPassword);
    }

    private void SeedTestData()
    {
        SeedCars();
        var carsQuery = "SET IDENTITY_INSERT dbo.Cars";
        SetIdentityOnAndSaveChanges(carsQuery);

        SeedContactDatas();
        var contactDatasQuery = "SET IDENTITY_INSERT dbo.ContactDatas";
        SetIdentityOnAndSaveChanges(contactDatasQuery);

		SeedRentalStores();
        var rentalStoresQuery = "SET IDENTITY_INSERT dbo.RentalStores";
        SetIdentityOnAndSaveChanges(rentalStoresQuery);

		SeedAvailableCars();
        var availableCarsQuery = "SET IDENTITY_INSERT dbo.AvailableCars";
		SetIdentityOnAndSaveChanges(availableCarsQuery);
	}

    private void SeedCars()
    {
        var cars = new List<Car>
        {
            new Car { Id = 1, Name = "Model S", Description = "The Model S is a luxury electric sedan with a range of up to 663 km on a single charge. It can accelerate from 0 to 100 km/h in just 1.99 seconds, making it one of the fastest production cars in the world.",
                DayRentalPrice = 80, ImageUrl = "\\img\\cars\\cd532321-c24e-427d-9be4-c285ef2720e7.png"},
            new Car { Id = 2, Name = "Model X", Description = "The Model X is a luxurious SUV with Falcon Wing doors and a range of up to 580 km on a single charge. It can accelerate from 0 to 100 km/h in just 2.6 seconds, making it one of the quickest SUVs on the market.",
                DayRentalPrice = 90, ImageUrl = "\\img\\cars\\394687b1-aab3-47bf-b1f9-a7a7eeefea29.png"},
            new Car { Id = 3, Name = "Model 3", Description = "The Model 3 is a premium electric sedan with a range of up to 580 km on a single charge. It can accelerate from 0 to 100 km/h in just 3.3 seconds, making it one of the quickest cars in its class.",
                DayRentalPrice = 70, ImageUrl = "\\img\\cars\\96eae141-729e-499a-8756-5c926950b331.png"},
            new Car { Id = 4, Name = "Model Y", Description = "The Model Y is a versatile electric crossover with a range of up to 525 km on a single charge. It can accelerate from 0 to 100 km/h in just 3.5 seconds, making it one of the fastest SUVs in its class.",
                DayRentalPrice = 85, ImageUrl = "\\img\\cars\\5763d428-e540-4005-9ee1-6f80c60caf4a.png"}
        };
        _dbContext.AddRange(cars);
    }

    private void SeedContactDatas()
    {
        var contactDatas = new List<ContactData>
        {
            new ContactData { Id = 1, City = "Palma de Mallorca", State = "Balearic Islands", StreetAddress = "Palma Airport, Car Rental Terminal",
                PostalCode = "07611", PhoneNumber = "+34 912 345 678", EmailAddress = "rentalcompanymallorca.airport@rental.com"},
            new ContactData { Id = 2, City = "Palma de Mallorca", State = "Balearic Islands", StreetAddress = "Carrer de la Seu, 12",
                PostalCode = "07001", PhoneNumber = "+34 912 345 679", EmailAddress = "rentalcompanymallorca.center@rental.com"},
            new ContactData { Id = 3, City = "Alcudia", State = "Balearic Islands", StreetAddress = "Carrer de Sant Jaume, 22",
                PostalCode = "07400", PhoneNumber = "+34 912 345 680", EmailAddress = "rentalcompanymallorca.alcudia@rental.com"},
            new ContactData { Id = 4, City = "Manacor", State = "Balearic Islands", StreetAddress = "Carrer de Rafa Nadal, 12",
                PostalCode = "07500", PhoneNumber = "+34 912 345 681", EmailAddress = "rentalcompanymallorca.manacor@rental.com"}
        };
        _dbContext.AddRange(contactDatas);
    }

    private void SeedRentalStores()
    {
        var rentalStores = new List<RentalStore>
        {
            new RentalStore { Id = 1, Name = "Palma Airport", ContactDataId = 1 },
            new RentalStore { Id = 2, Name = "Palma City Center", ContactDataId = 2 },
            new RentalStore { Id = 3, Name = "Alcudia", ContactDataId = 3 },
            new RentalStore { Id = 4, Name = "Manacor", ContactDataId = 4 },

        };
        _dbContext.AddRange(rentalStores);
    }

    private void SeedAvailableCars()
    {
        var availableCars = new List<AvailableCar>
        {
            new AvailableCar { Id = 1, CarId = 1, CarsCount = 1, RentalStoreId = 1 },
            new AvailableCar { Id = 2, CarId = 2, CarsCount = 1, RentalStoreId = 1 },
            new AvailableCar { Id = 3, CarId = 3, CarsCount = 1, RentalStoreId = 1 },
            new AvailableCar { Id = 4, CarId = 1, CarsCount = 1, RentalStoreId = 2 },
            new AvailableCar { Id = 5, CarId = 3, CarsCount = 1, RentalStoreId = 2 },
        };
        _dbContext.AddRange(availableCars);
    }

    private void SetIdentityOnAndSaveChanges(string sqlQuery)
    {
		_dbContext.Database.OpenConnection();
		try
        {
            _dbContext.Database.ExecuteSqlRaw(sqlQuery + " ON;");
            _dbContext.SaveChanges();
            _dbContext.Database.ExecuteSqlRaw(sqlQuery + " OFF;");
        }
        finally
        {
            _dbContext.Database.CloseConnection();
        }
    }
}
