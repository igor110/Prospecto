using Microsoft.AspNetCore.Mvc;
using Prospecto.Data;
using Prospecto.Models;
using System.Linq;

namespace Prospecto.Controllers
{
    public class MetaMensalController : Controller
    {
        private readonly ProspectoContext _context;

        public MetaMensalController(ProspectoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var metas = _context.MetasMensais.OrderByDescending(m => m.MesAno).ToList();
            return View(metas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MetaMensal model)
        {
            if (ModelState.IsValid)
            {
                // Evita duplicar meta para o mesmo mês
                var jaExiste = _context.MetasMensais.Any(m => m.MesAno.Month == model.MesAno.Month && m.MesAno.Year == model.MesAno.Year);
                if (!jaExiste)
                {
                    _context.MetasMensais.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Já existe uma meta cadastrada para este mês.");
            }

            return View(model);
        }
    }
}
