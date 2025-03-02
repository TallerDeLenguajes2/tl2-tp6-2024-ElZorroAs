using Microsoft.AspNetCore.Mvc;

namespace tl2_tp6_2024_ElZorroAs.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error403()
        {
            return View(); // Debe existir la vista Views/Error/Error403.cshtml
        }
    }
}
