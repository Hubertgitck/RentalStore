using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RentalCompanyWeb.Areas;
public static class TempDataHelper
{
    public static void SetSuccess(Controller controller, string message)
    {
        controller.TempData["success"] = message;
    }

    public static void SetSuccess(PageModel pageModel, string message)
    {
        pageModel.TempData["success"] = message;
    }

    public static void SetError(Controller controller, string message)
    {
        controller.TempData["error"] = message;
    }
}