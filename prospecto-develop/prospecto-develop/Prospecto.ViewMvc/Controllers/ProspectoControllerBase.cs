using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Prospecto.ViewMvc.Controllers
{
    public class ProspectoControllerBase : Controller
    {
        internal int UserId { get; private set; }
        internal string UserName { get; private set; }
        internal string Role { get; private set; }
        internal int CompanyId { get; private set; }
        internal int BranchId { get; private set; }

        protected string GetUserClaim(string claimType)
        {
            return User.Claims.FirstOrDefault(x => x.Type.Equals(claimType))?.Value;
        }

        public bool IsAuthenticate()
        {
            return User.Identity.IsAuthenticated;
        }

        public void GetContextData()
        {
            UserId = Convert.ToInt32(GetUserClaim(ClaimTypes.Sid));
            UserName = GetUserClaim(ClaimTypes.Name);
            Role = GetUserClaim(ClaimTypes.Role);
            var groupSid = GetUserClaim(ClaimTypes.GroupSid);
            if (!string.IsNullOrEmpty(groupSid))
                CompanyId = Convert.ToInt32(groupSid);

            var primarySid = GetUserClaim(ClaimTypes.PrimarySid);
            if (!string.IsNullOrEmpty(primarySid))
                BranchId = Convert.ToInt32(primarySid);
        }
    }
}
