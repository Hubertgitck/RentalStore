using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Application.Services;
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

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(RentalStoreDto rentalStoreDto)
    {
        if (ModelState.IsValid)
        {
            await _rentalStoreService.AddRentalStore(rentalStoreDto);
            TempData["success"] = "Rental Store added sucesfully";
            return RedirectToAction("Index");
        }
        return View(rentalStoreDto);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        return View(await _rentalStoreService.GetRentalStoreById(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(RentalStoreDto rentalStoreDto)
    {
        if (ModelState.IsValid)
        {
            await _rentalStoreService.EditRentalStore(rentalStoreDto);
            TempData["success"] = "Rental Store updated succesfully";
            return RedirectToAction("Index");
        }
        return View(rentalStoreDto);
    }
}
