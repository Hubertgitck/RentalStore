using Microsoft.AspNetCore.Mvc;

namespace RentalCompany.Areas.Customer.Controllers;

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
