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
    public class PeliculasController : Controller
    {
        private readonly CineDbContext _context;

        public PeliculasController(CineDbContext context)
        {
            _context = context;
        }

        // GET: Peliculas

        public async Task<IActionResult> Index()
        {
            var cineDbContext = _context.Peliculas.Include(p => p.Genero);
            return View(await cineDbContext.ToListAsync());
        }
        public IActionResult Funciones(Guid? id)
        {
            Pelicula pelicula = (Pelicula)_context.Peliculas
                .Include(p => p.Funciones).ThenInclude(p => p.Sala).ThenInclude(p => p.TipoSala)
                .FirstOrDefault(m=>m.ID==id);
            return View(pelicula);
        }
        

        // GET: Peliculas/Details/5
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .Include(p => p.Funciones).ThenInclude(p => p.Sala).ThenInclude(p=>p.TipoSala)
                .Include(p => p.Funciones).ThenInclude(p => p.Reservas)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // GET: Peliculas/Create
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create()
        {
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre");
            return View();
        }
        public decimal TotalPorMes(Pelicula pelicula)
        {
            decimal total = 0;
            foreach(Funcion funcion in pelicula.Funciones)
            {
                if (funcion.Fecha.Month == DateTime.Now.Month)
                {
                    foreach (Reserva reserva in funcion.Reservas)
                    {
                        total = total + reserva.CantidadButacas * funcion.Sala.TipoSala.Precio;

                    }
                }
            }

            return total;
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Create( Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                pelicula.ID = Guid.NewGuid();
                Console.WriteLine(pelicula.Genero);
              
                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
            return View(pelicula);
        }

        // GET: Peliculas/Edit/5
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
            return View(pelicula);
        }

        // POST: Peliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,FechaLanzamiento,Titulo,Descripcion,GeneroId")] Pelicula pelicula)
        {
            if (id != pelicula.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.ID))
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
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
            return View(pelicula);
        }

        // GET: Peliculas/Delete/5
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(Guid id)
        {
            return _context.Peliculas.Any(e => e.ID == id);
        }
    }
}
