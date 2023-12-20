using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practico.Data;
using Practico.Models;
using Practico.Types;

namespace Practico.Controllers
{
	public class InstanciaController : Controller
	{
		private readonly ApplicationDbContext _db;

        public InstanciaController(ApplicationDbContext db)
        {
			_db = db;
        }

        [Route("Instancia")]
        public async Task<IActionResult> Index(int? page = 1, string searchText = "")
        {
            
            var query = _db.Set<Instancia>().AsQueryable();
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(t =>
                    t.Nombre.Contains(searchText) ||
                    t.URL.Contains(searchText));
            }
            ViewBag.CurrentFilter = searchText;
            return View(await PaginatedList<Instancia>.CreateAsync(query.AsNoTracking(), page ?? 1, 10));
        }

        //GET
        public IActionResult Create()
		{
			return View();
		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
        public IActionResult Create(Instancia obj)
        {
			if (obj.Nombre == "Ortiba")
			{
				ModelState.AddModelError("Nombre", "Nombre no puede ser Ortiba.");
			}
			if (ModelState.IsValid)
			{
				_db.Instancias.Add(obj);
				_db.SaveChanges();
                TempData["success"] = "Instancia creada exitosamente";
				return RedirectToAction("Index");
			}
			return View(obj);
        }

        //GET
        public IActionResult Edit(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var instanciaFromDb = _db.Instancias.Find(Id);
            /* var instanciaFromDBFirst = _db.Instancias.FirstOrDefault(u=>u.Id==Id);
                        Trae el primero que encuentra. Ej: Busca por Temática.
                        Si más de uno tiene la misma, trae el primero que encuentra.
            var instanciaFromDBSingle = _db.Instancias.SingleOrDefault(u=>u.Id==Id);     
                        Tira exeption si hay más de un resultado. Ej: Busca por Temática.
                        Si dos tienen, da error */
            if (instanciaFromDb==null)
            {
                return NotFound();
            }

            return View(instanciaFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Instancia obj)
        {
            if (obj.Nombre == "Ortiba")
            {
                ModelState.AddModelError("Nombre", "Nombre no puede ser Ortiba.");
            }
            if (ModelState.IsValid)
            {
                _db.Instancias.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Instancia editada exitosamente";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var instanciaFromDb = _db.Instancias.Find(Id);
            /* var instanciaFromDBFirst = _db.Instancias.FirstOrDefault(u=>u.Id==Id);
                        Trae el primero que encuentra. Ej: Busca por Temática.
                        Si más de uno tiene la misma, trae el primero que encuentra.
            var instanciaFromDBSingle = _db.Instancias.SingleOrDefault(u=>u.Id==Id);     
                        Tira exeption si hay más de un resultado. Ej: Busca por Temática.
                        Si dos tienen, da error */
            if (instanciaFromDb == null)
            {
                return NotFound();
            }

            return View(instanciaFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? Id)
        {
            var instanciaFromDb = _db.Instancias.Find(Id);
            if (instanciaFromDb == null)
            {
                return NotFound();
            }

            _db.Instancias.Remove(instanciaFromDb);
            _db.SaveChanges();
            TempData["success"] = "Instancia borrada exitosamente";
            return RedirectToAction("Index");
        }
    }
}
