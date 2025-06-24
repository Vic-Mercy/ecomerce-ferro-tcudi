using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerroShop.Models.ViewModels
{
    public class ProcederConCompraViewModel
    {
        public CarritoViewModel Carrito { get; set; } = null!;

        public List<Direccion> Direcciones { get; set; } = null!;

        public int DireccionIdSelecciona{ get; set; }
        
    }
}