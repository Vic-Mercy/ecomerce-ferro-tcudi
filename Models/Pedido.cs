using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FerroShop.Models
{
    public class Pedido
    {
        [Key]

        public int PedidoId { get; set; }
        [Required]

        public int UsuarioId { get; set; } 
        [ForeignKey("UsuarioId")]

        public  Usuario Usuario  { get; set; } = null!;

        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public string Estado { get; set; } = null!;

        public int DireccionIdSeleccionada { get; set; }
       
        public  Direccion Direccion  { get; set; } = null!;


        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
           public ICollection<Detalle_Pedido> DetallesPedido {get; set;} = null!;


        
    }
}
