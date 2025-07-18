using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FerroShop.Data;
using FerroShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace FerroShop.Controllers
{
    [Authorize(Policy = "RequiredAdminorStaff")]
    public class UsuariosController : BaseController
    {


        public UsuariosController(ApplicationDbContext context) : base(context)
        {

        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Usuarios.Include(u => u.Rol);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Nombre,NombreUsuario,Contrasenia,Correo,Telefono,Direccion,Provincia,Localidad,CodigoPostal,RolId"
        )]
            Usuario usuario
            )

        {
            var rol = await _context.Roles
            .Where(d => d.RolId == usuario.RolId)
            .FirstOrDefaultAsync();


            if (rol != null)
            {
                usuario.Direcciones = new List<Direccion>
                {
                    new Direccion{
                        Address=usuario.Direccion,
                        Provincia=usuario.Provincia,
                        Localidad=usuario.Localidad,
                        CodigoPostal=usuario.CodigoPostal,
                    }
                };


                usuario.Rol = rol;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
        [Bind("UsuarioId,Nombre,NombreUsuario,Contrasenia,Correo,Telefono,Direccion,Provincia,Localidad,CodigoPostal,RolId")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            var rol = await _context.Roles
           .Where(d => d.RolId == usuario.RolId)
           .FirstOrDefaultAsync();

            if (rol != null)

            {


                var existingUser = await _context.Usuarios
                .Include(u => u.Direcciones)
                .FirstOrDefaultAsync(u => u.UsuarioId == id);

                if (existingUser != null)
                {
                    if (existingUser.Direcciones.Count > 0)
                    {
                        var direccion = existingUser.Direcciones.First();
                        direccion.Address = usuario.Direccion;
                        direccion.Provincia = usuario.Provincia;
                        direccion.Localidad = usuario.Localidad;
                        direccion.CodigoPostal = usuario.CodigoPostal;
                    }

                    else
                    {
                        existingUser.Direcciones = new List<Direccion>
                        {
                            new Direccion{
                                Address=usuario.Direccion,
                                Provincia=usuario.Provincia,
                                Localidad=usuario.Localidad,
                                CodigoPostal=usuario.CodigoPostal

                            }
                        };
                    }
                    usuario.Rol = rol;
                    existingUser.RolId = usuario.RolId;
                    existingUser.Nombre = usuario.Nombre;
                    existingUser.Telefono = usuario.Telefono;
                    existingUser.NombreUsuario = usuario.NombreUsuario;
                    existingUser.Contrasenia = usuario.Contrasenia;
                    existingUser.Correo = usuario.Correo;


                    try
                    {
                        _context.Update(existingUser);
                        await _context.SaveChangesAsync();
                    }

                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UsuarioExists(usuario.UsuarioId))
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

                return RedirectToAction(nameof(Index));
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
