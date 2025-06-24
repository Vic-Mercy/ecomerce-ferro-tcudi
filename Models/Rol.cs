using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FerroShop.Models
{

    public class Rol
    {
        [Key]
        public int RolId {get; set;}
         [Required(ErrorMessage = "El campo Nombre es Obligatorio")]
         [StringLength(50)]
        public string Nombre { get; set; } = null!;
    }

}
