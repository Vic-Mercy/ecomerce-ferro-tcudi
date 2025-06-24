using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Protocol.Plugins;

namespace FerroShop.Models
{
    public class Direccion
    {
        [Key]
        public int DireccionId { get; set; }
        
       [Required]
       [StringLength(50)]

       public string Address { get; set; } =null!;


        public string Provincia {get; set;} = null!;
         [Required]
        [StringLength(50)]

        public string Localidad {get; set;} = null!;
         [Required]
        [StringLength(10)]

        public string CodigoPostal {get; set;} = null!;

        [Required]
         public int UsuarioId { get; set; }
         [ForeignKey("UsuarioId")]

         public Usuario Usuario { get; set; } = null!;
    }
}
