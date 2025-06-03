using Microsoft.AspNetCore.Mvc;
using Prospecto.ViewMvc.Models;
using System.Diagnostics;

public class SharedController : Controller
{
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
