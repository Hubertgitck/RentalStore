using Microsoft.AspNetCore.Http;
using RentalCompany.Application.Dto;

namespace RentalCompany.Application.Interfaces;

public interface ICarService
{
    Task<IEnumerable<CarDto>> GetAllCars();
    Task AddCarToDatabase(CarDto carDto, IFormFile? file);
    Task<CarDto> GetCarById(int? id);
    Task Edit(CarDto carDto, IFormFile? file);
    Task DeleteCarById(int? id);
}