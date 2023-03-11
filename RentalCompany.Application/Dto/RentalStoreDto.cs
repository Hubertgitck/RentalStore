using System.ComponentModel.DataAnnotations;
using RentalCompany.Infrastructure.Models;

namespace RentalCompany.Application.Dto;

public class RentalStoreDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public ContactDataDto ContactDataDto { get; set; }
    public List<AvailableCar> AvailableCars { get; set; }
}
