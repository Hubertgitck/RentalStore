using Microsoft.AspNetCore.Mvc;

namespace RentalCompanyWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
	public HomeController()
	{

	}

	public IActionResult Index()
	{
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}
}
