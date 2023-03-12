namespace RentalCompany.Infrastructure.Models; 
public class ContactData 
{
    [Key]
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
    [EmailAddress]
    public string EmailAddress { get; set; }
}
