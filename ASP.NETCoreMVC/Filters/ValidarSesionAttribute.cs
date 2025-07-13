using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Session.GetString("Token");

            var rolUsuario = context.HttpContext.Session.GetString("Rol");

            var controller = context.Controller as Controller;

            if (controller != null)
            {
                if (string.IsNullOrEmpty(token))
                {
                    controller.TempData["Error"] = "Debes iniciar sesión para acceder a esta página";
                    context.Result = new RedirectToActionResult("Login", "Home", null);
                    return;
                }

                if (rolUsuario != "Administrador" && rolUsuario != "Funcionario" && rolUsuario != "Cliente")
                {
                    controller.TempData["Error"] = "No tienes permiso para acceder a esta página";
                    context.Result = new RedirectToActionResult("Login", "Home", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
