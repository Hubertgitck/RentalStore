using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RentalCompany.Infrastructure.Models;
public class RentHeader
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("ApplicationUserId")]
    public string ApplicationUserId { get; set; }
    [Required]
    [ValidateNever]
    public virtual ApplicationUser ApplicationUser { get; set; }
    [Required]
    [ForeignKey("CarId")]
    public int CarId { get; set; }
    public virtual Car Car { get; set; }

    [Required]
    public double TotalCost { get; set; }
    [Required]
    public string RentStatus { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

    public int PickupRentalStoreId { get; set; }
    public int ReturnRentalStoreId { get; set; }

    [ForeignKey("PickupRentalStoreId")]
    [InverseProperty("PickupPlaces")]
    public virtual RentalStore PickupRentalStore { get; set; }
    
    [ForeignKey("ReturnRentalStoreId")]
    [InverseProperty("ReturnPlaces")]

    public virtual RentalStore ReturnRentalStore { get; set; }

}
