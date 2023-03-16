using Microsoft.AspNetCore.Mvc;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;

namespace RentalCompany.Areas.Customer.Controllers;

[Area("Customer")]
public class ShopController : Controller
{
    private readonly IShopService _shopService;
    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _shopService.GetAllStores());
    }

    public async Task<IActionResult> Stock(int id)
    {
        return View(await _shopService.GetStockByRentalStoreId(id));
    }

    public async Task<IActionResult> Book(int carId, [FromQuery(Name = "storeId")] int storeId)
    {
        return View(await _shopService.GetBookViewByCarIdAndStoreId(carId, storeId));
    }

    public async Task<IActionResult> BookSummary(BookViewDto bookViewDto)
    {
        var rentHeaderId = await _shopService.AddOrderHeader(bookViewDto, User);
        return RedirectToAction("Summary", new { id = rentHeaderId });
    }
    public async Task<IActionResult> Summary(int id)
    {
        return View(await _shopService.GetRentHeaderById(id));
    }


}