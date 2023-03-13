using Microsoft.AspNetCore.Mvc;
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
}