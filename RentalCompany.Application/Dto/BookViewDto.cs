using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentalCompany.Infrastructure.Models;

namespace RentalCompany.Application.Dto;

public class BookViewDto
{
    public CarDto CarDto { get; set; }
    public RentHeaderDto RentHeaderDto { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem>? RentalStores { get; set; }

}
