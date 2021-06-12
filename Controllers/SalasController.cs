using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BENT1A.Grupo2.Database;
using Reserva_Cine.Models;
using Microsoft.AspNetCore.Authorization;
using BENT1A.Grupo2.Models.Enums;

namespace BENT1A.Grupo2.Controllers
{
    public class SalasController : Controller
    {
        private readonly CineDbContext _context;

        public SalasController(CineDbContext context)
        {
            _context = context;
        }

        // GET: Salas
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Index()
        {
            var cineDbContext = _context.Salas.Include(s => s.TipoSala);
            return View(await cineDbContext.ToListAsync());
        }

        // GET: Salas/Details/5
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas
                .Include(s => s.TipoSala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sala == null)
            {
                return NotFound();
            }

            return View(sala);
        }

        // GET: Salas/Create
        
        [Authorize(Roles = nameof(Rol.Administrador))]
        public IActionResult Create()
        {
            ViewData["TipoSalaID"] = new SelectList(_context.TipoSalas, "Id", "Nombre");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Create([Bind("Id,Numero,TipoSalaID,CapacidadButacas")] Sala sala)
        {
            if (ModelState.IsValid)
            {
                sala.Id = Guid.NewGuid();
                _context.Add(sala);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoSalaID"] = new SelectList(_context.TipoSalas, "Id", "Nombre", sala.TipoSalaID);
            return View(sala);
        }

        // GET: Salas/Edit/5
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas.FindAsync(id);
            if (sala == null)
            {
                return NotFound();
            }
            ViewData["TipoSalaID"] = new SelectList(_context.TipoSalas, "Id", "Nombre", sala.TipoSalaID);
            return View(sala);
        }

        // POST: Salas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Numero,TipoSalaID,CapacidadButacas")] Sala sala)
        {
            if (id != sala.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sala);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaExists(sala.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoSalaID"] = new SelectList(_context.TipoSalas, "Id", "Nombre", sala.TipoSalaID);
            return View(sala);
        }

        // GET: Salas/Delete/5
        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas
                .Include(s => s.TipoSala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sala == null)
            {
                return NotFound();
            }

            return View(sala);
        }

        // POST: Salas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sala = await _context.Salas.FindAsync(id);
            _context.Salas.Remove(sala);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaExists(Guid id)
        {
            return _context.Salas.Any(e => e.Id == id);
        }
    }
}
