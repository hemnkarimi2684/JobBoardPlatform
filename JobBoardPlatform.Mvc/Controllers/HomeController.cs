using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.Mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }

    public IActionResult NotFoundPage()
    {
        return View("NotFound");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
