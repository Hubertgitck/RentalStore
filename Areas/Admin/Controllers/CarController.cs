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
        return View(await _carService.GetAllCars());
    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(CarDto carDto, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            await _carService.AddCarToDatabase(carDto, file);
            TempDataHelper.SetSuccess(this, "Car added successfully");
            return RedirectToAction("Index");
        }
        else
        {
            return View(carDto);
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        return View(await _carService.GetCarById(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CarDto carDto, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            await _carService.Edit(carDto, file);
            TempDataHelper.SetSuccess(this, "Car created succesfully");
            return RedirectToAction("Index");
        }
        return View(carDto);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        return View(await _carService.GetCarById(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int? id)
    {
        await _carService.DeleteCarById(id);
        TempDataHelper.SetSuccess(this, "Car deleted succesfully");
        return RedirectToAction("Index");
    }
}
