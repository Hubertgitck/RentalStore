using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RentalCompany.Application.Dto;
public class RentHeaderDto
{
    public int Id { get; set; }
    //public string ApplicationUserId { get; set; }
    [ValidateNever]
    public ApplicationUserDto ApplicationUserDto { get; set; }
    [Required]
    public int CarId { get; set; }
    public CarDto CarDto { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please book something first!")]
    public double TotalCost { get; set; }
    [Required]
    public string RentStatus { get; set; }
    public string RentPaymentStatus { get; set; }
    public string? SessionId { get; set; }
    public string? PaymentIntendId { get; set; }
	public DateTime PaymentDate { get; set; }

	[Required]
    public DateTime StartDate { get; set; }
    [Required]
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