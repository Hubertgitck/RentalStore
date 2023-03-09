using System.ComponentModel.DataAnnotations.Schema;

namespace RentalCompany.Infrastructure.Models;

public class AvailableCar
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("CarId")]
    public int CarId { get; set; }
    public Car Car { get; set; }
    public int CarsCount { get; set; }
}
