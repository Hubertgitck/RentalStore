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
            TempDataHelper.SetSuccess(this, "Rental Store added sucesfully");
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
            TempDataHelper.SetSuccess(this, "Rental Store updated succesfully");
            return RedirectToAction("Index");
        }
        return View(rentalStoreDto);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        return View(await _rentalStoreService.GetRentalStoreById(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int? id)
    {
        await _rentalStoreService.DeleteRentalStoreById(id);
        TempDataHelper.SetSuccess(this, "Rental store deleted succesfully");
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> SelectCars(int? id)
    {
        var res = await _rentalStoreService.GetSelectCarsView(id);
        return View(await _rentalStoreService.GetSelectCarsView(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SelectCars(RentalStoreSelectDto rentalStoreSelectDto)
    {
        await _rentalStoreService.AddAvailableCars(rentalStoreSelectDto);
        return RedirectToAction("Index");
    }
}