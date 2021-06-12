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

namespace BENT1A.Grupo2.Controllers
{
    public class ClientesController : Controller
    {
        private readonly CineDbContext _context;

        public ClientesController(CineDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente,string pass)
        {

            try
            {
                pass.ValidarPassword();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Cliente.Password), ex.Message);
            }


            if (ModelState.IsValid)
            {
                cliente.ID = Guid.NewGuid();
                cliente.FechaUltimaModificacion = cliente.FechaAlta = cliente.FechaUltimoLoggin = DateTime.Now;
                cliente.Password = pass.Encriptar();
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                 return this.RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Cliente cliente,string pass)
        {

            if (!string.IsNullOrWhiteSpace(pass))
            {
                try
                {
                    pass.ValidarPassword();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(Cliente.Password), ex.Message);
                }
            }

            if (id != cliente.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var clienteDb = _context.Clientes.Find(id);

                    clienteDb.Nombre = cliente.Nombre;
                    clienteDb.Apellido = cliente.Apellido;
                    clienteDb.DNI = cliente.DNI;
                    clienteDb.Email = cliente.Email;
                    clienteDb.Direccion = cliente.Direccion;
                    clienteDb.Telefono = cliente.Telefono;

                    if (!string.IsNullOrWhiteSpace(pass))
                    {
                        clienteDb.Password = pass.Encriptar();
                    }
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(cliente);
            }
            return View(cliente);
        }

       // [Authorize(Roles = nameof(Rol.Cliente))]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        //[Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(Guid id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
