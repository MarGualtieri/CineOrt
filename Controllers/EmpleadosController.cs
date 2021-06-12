using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BENT1A.Grupo2.Database;
using Reserva_Cine.Models;
using usando_seguridad.Extensions;
using Microsoft.AspNetCore.Authorization;
using BENT1A.Grupo2.Models.Enums;

namespace BENT1A.Grupo2.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly CineDbContext _context;

        public EmpleadosController(CineDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Empleado,Administrador")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empleados.ToListAsync());
        }

        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.ID == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        
        [HttpGet]


        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Empleado,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Empleado empleado,string pass)
        {
            try
            {
                pass.ValidarPassword();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Empleado.Password), ex.Message);
            }


            if (ModelState.IsValid)
            {
                empleado.ID = Guid.NewGuid();
                empleado.Legajo = Guid.NewGuid();
                empleado.FechaUltimaModificacion = empleado.FechaAlta = empleado.FechaUltimoLoggin = DateTime.Now;
                empleado.Password = pass.Encriptar();
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Empleado empleado,string pass)
        {

            if (!string.IsNullOrWhiteSpace(pass))
            {
                try
                {
                    pass.ValidarPassword();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Empleado.Password), ex.Message);
                }

            }

                if (id != empleado.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var empleadoDb = _context.Empleados.FirstOrDefault(empleado => empleado.ID == id);

                    empleadoDb.Nombre = empleado.Nombre;
                    empleadoDb.Apellido = empleado.Apellido;
                    empleadoDb.DNI = empleado.DNI;
                    empleadoDb.Email = empleado.Email;
                    empleadoDb.Direccion = empleado.Direccion;
                    empleadoDb.Telefono = empleado.Telefono;

                    if (!string.IsNullOrWhiteSpace(pass))
                    {
                        empleadoDb.Password = pass.Encriptar();
                    }
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.ID))
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
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        [Authorize(Roles = nameof(Rol.Administrador))]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.ID == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }
        [Authorize(Roles = nameof(Rol.Administrador))]
        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(Guid id)
        {
            return _context.Empleados.Any(e => e.ID == id);
        }
    }
}
