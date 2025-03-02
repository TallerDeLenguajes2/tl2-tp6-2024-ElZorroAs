using repositoriosTP6;
using Microsoft.AspNetCore.Http;

namespace tl2_tp6_2024_ElZorroAs.Services
{
    public interface IAuthenticationService
    {
        bool Login(string username, string password);
        void Logout();
        bool IsAuthenticated();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsuariosRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext context;

        public AuthenticationService(IUsuariosRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            context = _httpContextAccessor.HttpContext;
        }

        public bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Usuario y contraseña no pueden estar vacíos.");
            }

            var user = _userRepository.ObtenerUsuario(username, password);
            if (user != null)
            {
                context.Session.SetString("IsAuthenticated", "true");
                context.Session.SetString("User", username);
                context.Session.SetString("AccessLevel", user.Rol); // Guarda "Administrador" o "Cliente"

                return true;
            }

            return false;
        }


        public void Logout()
        {
            context.Session.Remove("IsAuthenticated");
            context.Session.Remove("User");
            context.Session.Remove("AccessLevel");
        }

        public bool IsAuthenticated()
        {
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext no está disponible.");
            }

            return context.Session.GetString("IsAuthenticated") == "true";
        }
    }
}
