using System;
using System.Security.Claims;

namespace Prospecto.ViewMvc.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetCompanyId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst("CompanyId");
            return claim != null && int.TryParse(claim.Value, out int value) ? value : 0;
        }

        public static int? GetBranchId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst("BranchId");
            return claim != null && int.TryParse(claim.Value, out int value) ? value : (int?)null;
        }
    }
}
