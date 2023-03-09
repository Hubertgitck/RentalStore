﻿using System.ComponentModel.DataAnnotations.Schema;
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
    public ApplicationUser ApplicationUser { get; set; }
    [Required]
    [ForeignKey("CarId")]
    public int CarId { get; set; }
    public Car Car { get; set; }
    [Required]
    [ForeignKey("RentalStoreId")]
    public int PickupPlaceId {get; set; }
    public RentalStore PickupRentalStore { get; set; }
    [Required]
    [ForeignKey("RentalStoreId")]
    public int ReturnPlaceId { get; set; }
    public RentalStore ReturnRentalStore { get; set; }

    [Required]
    public double TotalCost { get; set; }
    [Required]
    public string RentStatus { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

}
