using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FerroShop.Models
{
    public class Categoria
    { 
        public Categoria()
        {
            Productos = new List<Producto>();
        }

        [Key]
        public int CategoriaId{ get; set; }


        [Required(ErrorMessage ="El campo Nombre es Obligatorio.")]
        [StringLength(255)]
        public String Nombre { get; set; } = null!;

        [Required(ErrorMessage ="El campo Descripcion es Obligatorio.")]
        [StringLength(255)]
        public String Descripcion { get; set; } = null!;
        
        
        public ICollection<Producto> Productos { get; set;} = null!;
    }
}
