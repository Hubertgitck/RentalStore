namespace RentalCompany.Infrastructure.Models; 
public class Car 
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double DayRentalPrice { get; set; }
    [Required]
    public string ImageUrl { get; set; }
}