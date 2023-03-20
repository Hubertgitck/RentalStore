using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompanyWeb.Areas;

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

	[Authorize]
	public async Task<IActionResult> Book(int carId, [FromQuery(Name = "storeId")] int storeId)
    {
        return View(await _shopService.GetBookViewByCarIdAndStoreId(carId, storeId));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
	[Authorize]
	public async Task<IActionResult> BookSummary(BookViewDto bookViewDto)
    {
        var rentHeaderId = await _shopService.AddOrderHeader(bookViewDto, User);
        return RedirectToAction("Summary", new { rentHeaderId });
    }

	[Authorize]
	public async Task<IActionResult> Summary(int rentHeaderId)
    {
        return View(await _shopService.GetRentHeaderById(rentHeaderId));
    }

	public async Task<IActionResult> Payment(int rentHeaderId)
    {
        var result = await _shopService.MakePayment(rentHeaderId, User);

        if (string.IsNullOrEmpty(result))
        {
            TempDataHelper.SetError(this, "There was an error with your rent, please try again.");
            return RedirectToAction("Index", "Shop");
        }
        else
        {
            Response.Headers.Add("Location", result);
            return new StatusCodeResult(303);
        }
    }

	[Authorize]
	public async Task<IActionResult> OrderConfirmation(int id)
    {
        await _shopService.OrderConfirmation(id);

        return View(id);
    }

	#region API CALLS
	[HttpGet]
	public async Task<IActionResult> GetCarAvailability(int carId, int storeId)
	{
		var result = await _shopService.GetCarAvalabilityByIdAndStore(carId, storeId);
		return Json(new { data = result });
	}
	#endregion
}