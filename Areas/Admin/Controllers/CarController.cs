using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Utility;

namespace RentalCompanyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.RoleAdmin + "," + Constants.RoleEmployee)]
public class CarController : Controller
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _carService.GetAllCars();
        return View(result);
    }

	public IActionResult Create()
	{
		return View();
	}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CarDto carDto, IFormFile? file) 
    {
        if (ModelState.IsValid)
        {
            await _carService.AddCarToDatabase(carDto, file);
            TempData["success"] = $"Product created successfully";
            return RedirectToAction("Index");
        }
        else
        {
            return View(carDto);
        }
    }

}

