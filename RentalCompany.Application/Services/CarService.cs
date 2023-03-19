using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Application.Middleware.CustomExceptions;
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
        carDto.ImageUrl = SaveImage(file);

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
            throw new NotFoundException($"Car with ID: {id} was not found in database");
        }

        var carDto = _mapper.Map<CarDto>(carFromDb);

        return Task.FromResult(carDto);
    }

	public Task Edit(CarDto carDto, IFormFile? file)
    {
        if (carDto.ImageUrl != null)
        {
            DeleteOldImage(carDto.ImageUrl);
        }
		carDto.ImageUrl = SaveImage(file);

        var carToEditInDb = _mapper.Map<Car>(carDto);
        _unitOfWork.Car.Update(carToEditInDb);
        _unitOfWork.Save();

        return Task.CompletedTask;
    }

    public Task DeleteCarById(int? id)
    {
        var carToDeleteInDb = _unitOfWork.Car.GetFirstOrDefault(u => u.Id == id);

        if (carToDeleteInDb == null)
        {
            throw new NotFoundException($"Car with ID: {id} was not found in database");
        }
        _unitOfWork.Car.Remove(carToDeleteInDb);
        _unitOfWork.Save();

        return Task.CompletedTask;
    }

	private string SaveImage(IFormFile? file)
	{
		string savedImageUrl = "";
        if (file != null)
        {
            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(_hostEnvironment.WebRootPath, @"img\cars");
            var extension = Path.GetExtension(file.FileName);

            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                file.CopyTo(fileStreams);
            }
            savedImageUrl = @"\img\cars\" + fileName + extension;
        }
		return savedImageUrl;
    }
    private void DeleteOldImage(string imgUrl)
    {
        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, imgUrl.TrimStart('\\'));
        if (File.Exists(oldImagePath))
        {
            File.Delete(oldImagePath);
        }
    }
}

