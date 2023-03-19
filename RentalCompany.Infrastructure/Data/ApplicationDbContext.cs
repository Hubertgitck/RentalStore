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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<RentHeader>()
            .HasOne(r => r.PickupRentalStore)
            .WithMany(s => s.PickupPlaces)
            .HasForeignKey(r => r.PickupRentalStoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<RentHeader>()
            .HasOne(r => r.ReturnRentalStore)
            .WithMany(s => s.ReturnPlaces)
            .HasForeignKey(r => r.ReturnRentalStoreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
