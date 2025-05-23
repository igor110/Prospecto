using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prospecto.Models.ViewModel;
using Prospecto.Service.Interface;
using Prospecto.ViewMvc.Models;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Prospecto.ViewMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;

        public AccountController(
            IUserService userService,
            ILogger<AccountController> logger)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Account/Login");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logar([Bind] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUser(model.Username, model.Password);
                if (user == null)
                {
                    TempData["error"] = "Email ou senha inválidos!";
                    return View("Login", model);
                }

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Name));
                identity.AddClaim(new Claim(ClaimTypes.Name, model.Username));
                identity.AddClaim(new Claim(ClaimTypes.Role, Enum.GetName(user.TypeUser)));
                identity.AddClaim(new Claim(ClaimTypes.Sid, user.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.GroupSid, user.CompanyId.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.PrimarySid, user.BranchId.ToString()));

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });

                TempData["success"] = "Bem vindo " + user.Name;
                return Redirect("/Home/Index");
            }
            else
            {
                return View("Login", model);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
