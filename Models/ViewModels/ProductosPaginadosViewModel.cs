using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerroShop.Models.ViewModels
{
    public class ProductosPaginadosViewModel
    {
        public List<Producto> Productos { get; set; } = null!;

        public int PaginaActual { get; set; }

        public int TotalPaginas { get; set; }

        public int? CategoriaIdSeleccionada { get; set; }

        public string? Busqueda { get; set; }

        public bool MostrarMensajeSinResultados { get; set; }
        
        public string? NombreCategoriaSeleccionada{ get; set; }
    }
   
}