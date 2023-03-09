using System.ComponentModel.DataAnnotations.Schema;

namespace RentalCompany.Infrastructure.Models;
public class RentalStore
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    [ForeignKey("ContactDataId")]
    public int ContactDataId { get; set; }
    public virtual ContactData ContactData { get; set; }

    public virtual List<AvailableCar> AvailableCars { get; set; }
    public virtual ICollection<RentHeader> PickupPlaces { get; set; }
    public virtual ICollection<RentHeader> ReturnPlaces { get; set; }
 

}
