using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace Prospecto.Filters.Exceptions
{
    public static class RouteExtensions
    {
        public static string GetArea(this RouteData routeData)
        {
            return routeData.Values["area"]?.ToString() ?? "";
        }

        public static string GetController(this RouteData routeData)
        {
            return routeData.Values["controller"]?.ToString() ?? "";
        }

        public static string GetAction(this RouteData routeData)
        {
            return routeData.Values["action"]?.ToString() ?? "";
        }

        public static string GetArea(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.RouteValues["area"]?.ToString() ?? "";
        }

        public static string GetController(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.RouteValues["controller"]?.ToString() ?? "";
        }

        public static string GetAction(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.RouteValues["action"]?.ToString() ?? "";
        }
    }
}
