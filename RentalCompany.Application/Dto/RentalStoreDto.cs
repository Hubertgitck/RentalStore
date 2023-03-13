using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentalCompany.Infrastructure.Models;

namespace RentalCompany.Application.Dto;

public class RentalStoreDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public ContactDataDto ContactDataDto { get; set; }
    [ValidateNever]
    public ICollection<AvailableCar> AvailableCars { get; set; }

}
