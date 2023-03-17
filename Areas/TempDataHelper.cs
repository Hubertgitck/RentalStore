using Microsoft.AspNetCore.Mvc;

namespace RentalCompanyWeb.Areas;

public static class TempDataHelper
{
    public static void SetSuccess(Controller controller, string message)
    {
        controller.TempData["success"] = message;
    }

    public static void SetError(Controller controller, string message)
    {
        controller.TempData["error"] = message;
    }
}