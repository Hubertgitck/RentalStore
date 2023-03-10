using System.ComponentModel.DataAnnotations;

namespace RentalCompany.Application.Dto;

public class CarDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double DayRentalPrice { get; set; }
}
