using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RentalCompany.Application.Dto;

public class RentalStoreSelectDto
{
    public RentalStoreDto RentalStoreDto { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Only positive numbeers allowed!")]
    public Dictionary<int, int> NumberOfAvailableCars { get; set; }
    [ValidateNever]
    public ICollection<CarDto> CarsList { get; set; }
}