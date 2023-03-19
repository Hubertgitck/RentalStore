using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCompany.Application.Interfaces;
using RentalCompany.Utility;

namespace RentalCompanyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.RoleAdmin + "," + Constants.RoleEmployee)]
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

    #region API CALLS
    [HttpGet]
    public async Task<IActionResult> GetAll(string status)
    {
        var result = await _orderService.GetAllOrders(User, status);
        return Json(new { data = result });
    }
    #endregion
}
