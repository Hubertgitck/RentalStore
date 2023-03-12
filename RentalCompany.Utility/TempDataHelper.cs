using System.Web.Mvc;

namespace RentalCompany.Utility;
public static class TempDataHelper
{
    public static void SetSuccess(Controller controller, string message)
    {
        controller.TempData["success"] = message;
    }
}
