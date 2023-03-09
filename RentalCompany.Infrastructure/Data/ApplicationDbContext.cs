using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RentalCompany.Infrastructure.Data;

public class ApplicationDbContext :IdentityDbContext
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<AvailableCar> AvailableCars { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<ContactData> ContactDatas { get; set; }
    public DbSet<RentalStore> RentalStores { get; set;}
    public DbSet<RentHeader> RentHeaders { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
}
