using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FerroShop.Data;
using FerroShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace FerroShop.Controllers
{
    [Authorize(Policy = "RequiredAdminorStaff")]
    public class DireccionesController : BaseController
    {

        public DireccionesController(ApplicationDbContext context) : base(context)
        {

        }

        // GET: Direcciones
        public async Task<IActionResult> Index()
        {
            var direcciones = await _context.Direcciones
                .Include(d => d.Usuario)
                .ToListAsync();

            return View(direcciones);
        }

        // GET: Direcciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.DireccionId == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // GET: Direcciones/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "Nombre");
            return View();
        }

        // POST: Direcciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DireccionId,Address,Provincia,Localidad,CodigoPostal,UsuarioId")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(direccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios,
                "UsuarioId",
                "Nombre",
                direccion.UsuarioId
            );
            return View(direccion);
        }

        // GET: Direcciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var direccion = await _context.Direcciones
            .Include(d => d.Usuario)
           .FirstOrDefaultAsync(d => d.DireccionId == id);

            if (direccion == null)
                return NotFound();

            ViewData["UsuarioId"] = new SelectList(
          _context.Usuarios,
           "UsuarioId",
          "Nombre",
         direccion.UsuarioId
         );

            return View(direccion);
        }
        // POST: Direcciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DireccionId,Address,Provincia,Localidad,CodigoPostal,UsuarioId")] Direccion direccion)
        {
            if (id != direccion.DireccionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(direccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DireccionExists(direccion.DireccionId))
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
            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios,
                "UsuarioId",
                "Nombre",
                direccion.UsuarioId
            );
            return View(direccion);
        }

        // GET: Direcciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .FirstOrDefaultAsync(m => m.DireccionId == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // POST: Direcciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var direccion = await _context.Direcciones.FindAsync(id);
            if (direccion != null)
            {
                _context.Direcciones.Remove(direccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DireccionExists(int id)
        {
            return _context.Direcciones.Any(e => e.DireccionId == id);
        }
    }
}
