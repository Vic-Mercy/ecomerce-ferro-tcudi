using System.Security.Claims;
using FerroShop.Data;
using FerroShop.Models;
using FerroShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FerroShop.Controllers
{
    public class CarritoController : BaseController
    {
        public CarritoController(ApplicationDbContext context) : base(context) { }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var carritoViewModel = await GetCarritoViewModelAsync();

            var itemsEliminar = new List<CarritoItemViewModel>();

            foreach (var item in carritoViewModel.Items.ToList())
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto != null)
                {
                    item.Producto = producto;
                    if (!producto.Activo)
                    {
                        itemsEliminar.Add(item);
                    }

                    else
                    {
                        item.Cantidad = Math.Min(item.Cantidad, producto.Stock);
                    }
                }
                else
                {
                    itemsEliminar.Add(item);
                }
            }
            foreach (var item in itemsEliminar)

                carritoViewModel.Items.Remove(item);
            await UpdateCarritoViewModelAsync(carritoViewModel);


            carritoViewModel.Total = carritoViewModel.Items.Sum(item => item.Subtotal);

            var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int usuarioId = int.TryParse(claimValue, out var id) ? id : 0;

            var direcciones = User.Identity?.IsAuthenticated == true
                ? _context.Direcciones.Where(d => d.UsuarioId == usuarioId).ToList()
                : new List<Direccion>();

            var procederConCompraViewModel = new ProcederConCompraViewModel
            {
                Carrito = carritoViewModel,
                Direcciones = direcciones
            };

            return View(procederConCompraViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarCantidad(int id, int cantidad)
        {
            var carritoViewModel = await GetCarritoViewModelAsync();
            var carritoItem = carritoViewModel.Items.FirstOrDefault(i => i.ProductoId == id);

            if (carritoItem != null)
            {
                carritoItem.Cantidad = cantidad;
                var producto = await _context.Productos.FindAsync(id);

                if (producto != null && producto.Activo && producto.Stock > 0)
                    carritoItem.Cantidad = Math.Min(cantidad, producto.Stock);

                await UpdateCarritoViewModelAsync(carritoViewModel);
            }

            return RedirectToAction("Index", "Carrito");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var carritoViewModel = await GetCarritoViewModelAsync();
            var carritoItem = carritoViewModel.Items.FirstOrDefault(i => i.ProductoId == id);

            if (carritoItem != null)
            {
                carritoViewModel.Items.Remove(carritoItem);
                await UpdateCarritoViewModelAsync(carritoViewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> VaciarCarrito()
        {
            await Task.Run(() => Response.Cookies.Delete("carrito"));
            return RedirectToAction("Index");
        }

        private readonly string clientId = "";
        private readonly string clientSecret = "";

        [HttpPost]


        public async Task<IActionResult> ProcederConCompra(decimal montoTotal, int direccionIdSeleccionada)
        {
            if (direccionIdSeleccionada <= 0)
                return RedirectToAction("Index");

            var carritoViewModel = await GetCarritoViewModelAsync();

            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int usuarioId = int.TryParse(claim, out int uid) ? uid : 0;

            // Creamos el pedido en estado Pendiente
            var pedido = new Pedido
            {
                UsuarioId = usuarioId,
                Fecha = DateTime.UtcNow,
                Estado = "Pendiente",
                DireccionIdSeleccionada = direccionIdSeleccionada,
                Total = carritoViewModel.Total
            };

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

      
            foreach (var item in carritoViewModel.Items)
            {
                var detalle = new Detalle_Pedido
                {
                    PedidoId = pedido.PedidoId,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    Precio = item.Precio
                };
                _context.DetallePedidos.Add(detalle);
            }

            _context.SaveChanges();

           
            var items = carritoViewModel.Items.Select(item => new

            {
                title = item.Producto?.Nombre ?? "Producto",
                quantity = item.Cantidad,
                currency_id = "ARS",
                unit_price = item.Cantidad > 0 ? Math.Round(item.Subtotal / item.Cantidad, 2) : 0
            }).ToList();

            var body = new
            {

                items = items,
                back_urls = new
                {
                    success = "https://localhost:5001/Carrito/PagoCompletado",
                    failure = "https://localhost:5001/Carrito/Error",
                    pending = "https://localhost:5001/Carrito/Pendiente"
                },
                auto_return = "approved",
                external_reference = pedido.PedidoId.ToString()
            };

            var accessToken = "APP_USR-...";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.mercadopago.com/checkout/preferences", content);
            var json = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(json);
            string urlPago = result.init_point;

            return Redirect(urlPago);
        }



            public IActionResult Error()
           {
              return View(); 
            }

            public IActionResult Pendiente()
          {
            return View(); 
           }


        public async Task<IActionResult> PagoCompletado(string external_reference)
        {
            if (string.IsNullOrEmpty(external_reference))
                return RedirectToAction("Error");

            if (!int.TryParse(external_reference, out int pedidoId))
                return RedirectToAction("Error");

            var pedido = await _context.Pedidos
            .Include(p => p.DetallesPedido)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);

            if (pedido == null)
                return RedirectToAction("Error");

            
            pedido.Estado = "Vendido";

            
            foreach (var detalle in pedido.DetallesPedido)


            {
                var producto = await _context.Productos.FirstOrDefaultAsync(p => p.ProductoId == detalle.ProductoId);
                if (producto != null)
                    producto.Stock -= detalle.Cantidad;
            }

            await _context.SaveChangesAsync();

            ViewBag.DetallePedidos = pedido.DetallesPedido;

            return View("PagoCompletado", pedido);
        }


    }
    
  }

