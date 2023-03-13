using System.ComponentModel.DataAnnotations.Schema;

namespace RentalCompany.Infrastructure.Models;

public class AvailableCar
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("CarId")]
    public int CarId { get; set; }
    public virtual Car Car { get; set; }
    public int CarsCount { get; set; }

    [ForeignKey("RentalStoreId")]
    [InverseProperty("AvailableCars")]
    public virtual RentalStore AvailableCars { get; set; }
    [ForeignKey("RentalStoreId")]
    public int RentalStoreId { get; set; } 
}
