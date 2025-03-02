using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging; // Asegúrate de importar el espacio de nombres para el logger
using tl2_tp6_2024_ElZorroAs.Services;
using tl2_tp6_2024_ElZorroAs.ViewModel;

namespace tl2_tp6_2024_ElZorroAs.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogger<LoginController> _logger; // Logger

        public LoginController(IAuthenticationService authService, ILogger<LoginController> logger)
        {
            _authService = authService;
            _logger = logger; // Inicializamos el logger
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar iniciar sesión."); // Logueo del error
                model.ErrorMessage = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.";
                return View(model); // Retorna la vista con el mensaje de error
            }
        }

        public IActionResult Logout()
        {
            try
            {
                _authService.Logout(); // Llamar al servicio para limpiar la sesión
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar cerrar sesión."); // Logueo del error
                return View("Error"); // Retorna una vista de error
            }
        }
    }
}