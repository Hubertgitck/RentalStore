using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
	[Required]
	[ValidateNever]
	public string ImageUrl { get; set; }
}