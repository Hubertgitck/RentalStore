using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}

