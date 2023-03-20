namespace RentalCompany.Infrastructure.Models; 
public class Car 
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    [Required]
    [StringLength(100)]
    public string Description { get; set; }
    [Required]
    public double DayRentalPrice { get; set; }
    [Required]
    public string ImageUrl { get; set; }
}