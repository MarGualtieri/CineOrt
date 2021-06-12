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
using System.Security.Claims;

namespace BENT1A.Grupo2.Controllers
{
    public class ReservasController : Controller
    {
        private readonly CineDbContext _context;

        public ReservasController(CineDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var cineDbContext = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcion);
            Console.WriteLine(cineDbContext);
            return View(await cineDbContext.ToListAsync());
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        public IActionResult ReservaClienteFirst()
        {
            var clienteId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var tieneReserva = _context.Reservas.Where(reserva => reserva.ClienteId == clienteId).Where(reserva => reserva.Estado == EstadoReserva.Activa).ToList().Count;
            ViewBag.tieneReserva = tieneReserva;
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "ID", "Titulo");
            return View();
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        public IActionResult ReservaClienteSecond(Guid? PeliculaId)
        {
            ViewBag.PeliculaId = PeliculaId;


            return View();
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        public IActionResult ReservaClienteThird(Guid PeliculaId, String Fecha, String Hora,int CantidadButacas)
        {
            var funciones = _context.Funciones
                .Where(funcion => funcion.ButacasDisponibles >= CantidadButacas)
                .Where(funcion => funcion.Confirmada == true).Where(funcion => funcion.PeliculaId == PeliculaId);




            var cantFunciones = funciones.ToList().Count;
            ViewBag.cantFunciones = cantFunciones;
            ViewBag.CantidadButacas = CantidadButacas;



            ViewData["FuncionId"] = new SelectList((from funcion in funciones.ToList()
            select new
            {
                funcionId = funcion.Id,
                fecha = funcion.Hora.ToShortTimeString() + " - " + funcion.Fecha.ToShortDateString()
            }), "funcionId", "fecha", null);
            

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Rol.Cliente))]
        public IActionResult ReservaClienteCreate(int CantidadButacas, Guid FuncionId)
        {

            if (ModelState.IsValid)
            {

                Reserva reserva = new Reserva
                {
                    Id = Guid.NewGuid(),
                    ClienteId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    Estado = EstadoReserva.Activa,
                    FechaAlta = DateTime.Now,
                    CantidadButacas= CantidadButacas,
                    FuncionId = FuncionId
                };


                var funcion = _context.Funciones.Find(reserva.FuncionId);
               funcion.ButacasDisponibles -= reserva.CantidadButacas;

                _context.Add(reserva);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }


            return View();
        }



            // GET: Reservas/Details/5
            public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        public IActionResult Create()
        {
            Console.WriteLine("creando");
            ViewData["FuncionId"] = new SelectList(_context.Funciones, nameof(Funcion.Id), nameof(Funcion.Descripcion));
            return View();
        }


        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Reserva reserva)
        {
            Console.WriteLine(reserva.FuncionId);
            if (ModelState.IsValid)
            {
                var reservaactiva = false;
                var persona = _context.Clientes
                    .Include(p => p.Reservas)
                    .FirstOrDefault(m => m.ID == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                var funcion = _context.Funciones.Find(reserva.FuncionId);
                foreach (Reserva reservy in persona.Reservas)
                {
                    if (reservy.Estado == EstadoReserva.Activa)
                    {
                        reservaactiva = true;
                    }
                }
                if (funcion.ButacasDisponibles < reserva.CantidadButacas)
                {
                    ModelState.AddModelError(nameof(Reserva.CantidadButacas), "La funcion solo tiene " + funcion.ButacasDisponibles + " butcas disponibles");
                }
                if (reservaactiva)
                {
                    ModelState.AddModelError(nameof(Reserva.CantidadButacas), "No puede hacer una reserva si ya posee una reserva activa.");
                }
                else
                {
                    reserva.Id = Guid.NewGuid();
                    reserva.ClienteId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    reserva.Estado = EstadoReserva.Activa;
                    reserva.FechaAlta = DateTime.Now;
                    funcion.ButacasDisponibles -= reserva.CantidadButacas;
                    _context.Add(reserva);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

            }
            ViewData["FuncionId"] = new SelectList(_context.Funciones, nameof(Funcion.Id), nameof(Funcion.Descripcion), reserva.FuncionId);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ID", "Apellido", reserva.ClienteId);
            ViewData["FuncionId"] = new SelectList(_context.Funciones, "ID", "PeliculaId", reserva.FuncionId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FuncionId,FechaAlta,ClienteId,CantidadButacas,Estado")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ID", "Apellido", reserva.ClienteId);
            ViewData["FuncionId"] = new SelectList(_context.Funciones, "ID", "PeliculaId", reserva.FuncionId);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpGet]
        public IActionResult MisReservas()
        {
            var clienteId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var reservas = _context.Reservas
                .Include(reserva => reserva.Funcion).ThenInclude(funcion => funcion.Pelicula)
                .Include(reserva => reserva.Funcion).ThenInclude(funcion => funcion.Sala).ThenInclude(sala => sala.TipoSala)
                .Where(reserva => reserva.ClienteId == clienteId)
                .ToList();

            return View(reservas);
        }

        private bool ReservaExists(Guid id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}
