using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Utility;

namespace RentalCompanyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.RoleAdmin + "," + Constants.RoleEmployee)]
public class RentalStoreController : Controller
{
    private readonly IRentalStoreService _rentalStoreService;

    public RentalStoreController(IRentalStoreService rentalStoreService)
    {
        _rentalStoreService = rentalStoreService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _rentalStoreService.GetAllRentalStores());
    }
}
