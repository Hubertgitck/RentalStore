using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Infrastructure.Models;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Application.Services;

public class CarService : ICarService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IWebHostEnvironment _hostEnvironment;


	public CarService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment hostEnvironment)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_hostEnvironment = hostEnvironment;
	}

	public Task<IEnumerable<CarDto>> GetAllCars()
	{
		var allCarsCollection = _unitOfWork.Car.GetAll();
		var allCarsCollectionDto = _mapper.Map<IEnumerable<CarDto>>(allCarsCollection);

		return Task.FromResult(allCarsCollectionDto);
	}

	public Task AddCarToDatabase(CarDto carDto, IFormFile? file)
	{
		string wwwRootPath = _hostEnvironment.WebRootPath;

		if (file != null)
		{
			string fileName = Guid.NewGuid().ToString();
			var uploads = Path.Combine(wwwRootPath, @"img\cars");
			var extension = Path.GetExtension(file.FileName);

			using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
			{
				file.CopyTo(fileStreams);
			}
			carDto.ImageUrl = @"\img\cars\" + fileName + extension;
		}

		var carToDb = _mapper.Map<Car>(carDto);

		_unitOfWork.Car.Add(carToDb);
		_unitOfWork.Save();

		return Task.CompletedTask;
	}

	public Task<CarDto> GetCarById(int? id)
	{
        if (id.GetValueOrDefault() == 0)
        {
            throw new ArgumentException("Invalid id");
        }

        var carFromDb = _unitOfWork.Car.GetFirstOrDefault(c => c.Id == id);

        if (carFromDb == null)
		{
            throw new Exception($"Car with ID: {id} was not found in database");
        }

        var carDto = _mapper.Map<CarDto>(carFromDb);

        return Task.FromResult(carDto);
    }
}

