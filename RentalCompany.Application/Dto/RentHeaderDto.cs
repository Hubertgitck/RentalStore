using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentalCompany.Infrastructure.Models;

namespace RentalCompany.Application.Dto;
public class RentHeaderDto
{
    public string ApplicationUserId { get; set; }
    [Required]
    [ValidateNever]
    public virtual ApplicationUser ApplicationUser { get; set; }
    [Required]
    public int CarId { get; set; }
    public CarDto CarDto { get; set; }

    [Required]
    public double TotalCost { get; set; }
    [Required]
    public string RentStatus { get; set; }
    [Required]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateTime StartDate { get; set; }
    [Required]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateTime EndDate { get; set; }
    [Required]
    [Display(Name = "Pickup rental store")]
    public int PickupRentalStoreId { get; set; }
    [Required]
    [Display(Name = "Return rental store")]
    public int ReturnRentalStoreId { get; set; }

    public RentalStoreDto PickupRentalStore { get; set; }

    public RentalStoreDto ReturnRentalStore { get; set; }

}
