using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace FerroShop.Models
{
    public class Producto
    {
        [Key]
        public int ProductoId { get;set; }
        [Required]
        [StringLength(100)]
        public string Codigo { get;set; } = null!;
        [Required]
        [StringLength(255)]

        public string Nombre { get;set; } = null!;
        [Required]
        [StringLength(1000)]
        public string Modelo { get;set; } = null!;
        [Required]
        [StringLength(255)]

        public string Descripcion { get;set; } = null!;

       [Required]
       [Column(TypeName = "decimal(18,2)")]
       [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
       public decimal Precio { get; set; }



        [Required]
        [StringLength(255)]

        public string Imagen { get;set; } = null!;
        [Required]
        
        public int CategoriaId { get;set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get;set; } = null!;
        [Required]
        
        public int Stock { get;set; }
        [Required]
        [StringLength(255)]
        
        public string Marca { get;set; } = null!;
        [Required]
        public bool Activo { get;set; } 
        public ICollection<Detalle_Pedido> DetallesPedido {get; set;} = null!;

    }
}
