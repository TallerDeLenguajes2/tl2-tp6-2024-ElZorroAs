using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using tl2_tp6_2024_ElZorroAs.Services;
using tl2_tp6_2024_ElZorroAs.ViewModel;

namespace tl2_tp6_2024_ElZorroAs.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authService;

        public LoginController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validar que no estén vacíos
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                model.ErrorMessage = "El usuario y la contraseña son obligatorios.";
                return View(model);
            }

            // Intentar iniciar sesión
            if (_authService.Login(model.Username, model.Password))
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");

                // Obtener el nivel de acceso de la sesión
                var accessLevel = HttpContext.Session.GetString("AccessLevel") ?? "Cliente"; // "Cliente" por defecto
                HttpContext.Session.SetString("AccessLevel", accessLevel);

                return RedirectToAction("ListarPresupuesto", "Presupuestos");
            }

            model.ErrorMessage = "Usuario o contraseña incorrectos.";
            return View(model);
        }

        public IActionResult Logout()
        {
            _authService.Logout(); // Llamar al servicio para limpiar la sesión
            return RedirectToAction("Index");
        }
    }
}
