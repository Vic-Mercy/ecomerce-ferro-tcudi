using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FerroShop.Data;
using FerroShop.Models;
using Microsoft.AspNetCore.Authorization;
using FerroShop.Services;

namespace FerroShop.Controllers
{
    [Authorize(Policy = "RequiredAdminorStaff")]
    public class ProductosController : BaseController
    {
        private readonly IProductoService _productoService;

        public ProductosController(ApplicationDbContext context, IProductoService productoService)
           : base(context)
        {
            _productoService = productoService;
        }

        // GET: Productos
        public async Task<IActionResult> Index(string? busqueda, int pagina = 1)

        {
            int productosPorPagina = 10;

            var viewModel = await _productoService.GetProductosPaginados(
            categoriaId: null,
            busqueda: busqueda,
            pagina: pagina,
            productosPorPagina: productosPorPagina
         );

            return View(viewModel);

        }


        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Descripcion");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "ProductoId,Codigo,Nombre,Modelo,Descripcion,Precio,Imagen,CategoriaId,Stock,Marca,Activo"
                )] Producto producto

            )
        {

            var cat = await _context.Categorias
            .Where(c => c.CategoriaId == producto.CategoriaId)
            .FirstOrDefaultAsync();

            if (cat != null)
            {
                producto.Categoria = cat;
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(
            _context.Categorias, "CategoriaId", "Nombre",
            producto.CategoriaId

        ); return View(producto);

        }



        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Descripcion", producto.CategoriaId);
            return View(producto);

        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
       [Bind("ProductoId,Codigo,Nombre,Modelo,Descripcion,Precio,Imagen,CategoriaId,Stock,Marca,Activo")] Producto producto)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            var productoExistente = await _context.Productos.FindAsync(id);
            if (productoExistente == null)
            {
                return NotFound();
            }


            productoExistente.Codigo = producto.Codigo;
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Modelo = producto.Modelo;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;
            productoExistente.Imagen = producto.Imagen;
            productoExistente.CategoriaId = producto.CategoriaId;
            productoExistente.Stock = producto.Stock;
            productoExistente.Marca = producto.Marca;
            productoExistente.Activo = producto.Activo;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(producto.ProductoId))
                {
                    return NotFound();
                }
                else
                {

                    ViewData["CategoriaId"] = new SelectList(
                        _context.Categorias, "CategoriaId", "Descripcion", producto.CategoriaId
                    );
                    return View(producto);
                }
            }
        }



        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }

    
        
    
    }
      

}
