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
    public string StreetAddress { get; set; }
    public string PostalCode { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
}
