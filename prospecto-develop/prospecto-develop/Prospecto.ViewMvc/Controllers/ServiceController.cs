using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prospecto.ViewMvc.Models;
using System.Diagnostics;

namespace Prospecto.ViewMvc.Controllers
{
    public class ServiceController : ProspectoControllerBase
    {
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(ILogger<ServiceController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
