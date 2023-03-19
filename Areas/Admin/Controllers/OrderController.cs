using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;

namespace RentalCompanyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Details(int rentHeaderId)
    {
        return View(await _orderService.GetRentHeaderById(rentHeaderId));
    }

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> UpdateOrderDetail(RentHeaderDto rentHeaderDto)
	{
		var result = await _orderService.UpdateRentHeader(rentHeaderDto);
		TempDataHelper.SetSuccess(this, "Rent Details Updated Successfully");
		return RedirectToAction("Details", "Order", new { rentHeaderId = result });
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> CancelOrder(RentHeaderDto rentHeaderDto)
	{
		await _orderService.CancelOrder(rentHeaderDto.Id);

		TempDataHelper.SetSuccess(this, "Rent Cancelled Successfully");
		return RedirectToAction("Details", "Order", new { rentHeaderId = rentHeaderDto.Id });
	}

	#region API CALLS
	[HttpGet]
    public async Task<IActionResult> GetAll(string status)
    {
        var result = await _orderService.GetAllOrders(User, status);
        return Json(new { data = result });
    }
    #endregion
}
