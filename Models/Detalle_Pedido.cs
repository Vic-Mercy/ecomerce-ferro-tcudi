using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FerroShop.Models
{
    public class Detalle_Pedido
    {
        [Key]
       public int DetallePedido {get; set;}
       [Required]
      public int PedidoId{ get; set;}
       [ForeignKey("PedidoId")]

       public Pedido Pedido { get; set; } = null!;

       [Required]
       public int ProductoId{get; set;}
       [ForeignKey("ProductoId")]
        public Producto Producto { get; set; } = null!;
        [Required]

        public int Cantidad{ get; set;}
        [Required]

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

    }
}
