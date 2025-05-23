using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Prospecto.Filters.Exceptions;
using Prospecto.Models.Enums;
using System.Linq;

namespace Prospecto.Filters
{
    public class ClaimsRequirementFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool anon = context.ActionDescriptor.EndpointMetadata.Any(en => en.GetType() == typeof(AllowAnonymousAttribute));
            if (anon)
                return;

            if (!context.HttpContext.User.Identity.IsAuthenticated && !(context.ActionDescriptor.GetController() == "Account"))
            {
                context.Result = new RedirectResult("/Account/Login");
                return;
            }

            if (HavePermissao(context))
                return;
            else
                context.Result = new RedirectResult("/Account/Login");
        }

        private static bool HavePermissao(AuthorizationFilterContext context)
        {
            string controller = "";

            if (context.HttpContext.User.IsInRole(UserTypeEnum.CONSULTANT.ToString()))
            {
                controller = context.ActionDescriptor.GetController();
                if (controller == "User" || controller == "Branch" || controller == "AttendanceImport" || controller == "Company")
                    return false;

                return true;
            }

            if (context.HttpContext.User.IsInRole(UserTypeEnum.MANAGER.ToString()))
            {
                controller = context.ActionDescriptor.GetController();
                if (controller == "Company")
                    return false;

                return true;
            }

            return true;
        }
    }
}
