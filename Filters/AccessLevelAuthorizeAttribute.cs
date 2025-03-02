using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AccessLevelAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _requiredAccessLevels;

    // Constructor que recibe los niveles de acceso permitidos
    public AccessLevelAuthorizeAttribute(params string[] requiredAccessLevels)
    {
        _requiredAccessLevels = requiredAccessLevels ?? Array.Empty<string>();
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Primero verificamos si el usuario está autenticado
        if (!IsAuthenticated(context))
        {
            // Si no está autenticado, redirigir a la página de login
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        // Obtener nivel de acceso del usuario
        var userAccessLevel = GetUserAccessLevel(context);

        // Si el usuario no tiene el nivel de acceso adecuado, redirigir al error 403
        if (!_requiredAccessLevels.Contains(userAccessLevel, StringComparer.OrdinalIgnoreCase))
        {
            context.Result = new RedirectToActionResult("Error403", "Error", null);
            return;
        }

        // Si está autenticado y tiene el acceso adecuado, la acción continuará.
    }

    private static string GetUserAccessLevel(AuthorizationFilterContext context)
    {
        return context.HttpContext.Session.GetString("AccessLevel") ?? string.Empty;
    }

    private static bool IsAuthenticated(AuthorizationFilterContext context)
    {
        return context.HttpContext.Session.GetString("IsAuthenticated") == "true";
    }
}
