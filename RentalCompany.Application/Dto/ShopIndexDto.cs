namespace RentalCompany.Application.Dto;

public class ShopIndexDto
{
    public string RentalStoreName { get; set; }
    public IEnumerable<CarDto> CarsInStock { get; set; }
}
