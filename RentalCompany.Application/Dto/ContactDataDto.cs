using System.ComponentModel.DataAnnotations;

namespace RentalCompany.Application.Dto;

public class ContactDataDto
{
    public int Id { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string State { get; set; }
    [Required]
    [Display(Name = "Street address")]
    public string StreetAddress { get; set; }
    [Display(Name = "Postal code")]
    public string PostalCode { get; set; }
    [Required]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }
    [EmailAddress]
    [Display(Name = "Email address")]
    public string EmailAddress { get; set; }
}
