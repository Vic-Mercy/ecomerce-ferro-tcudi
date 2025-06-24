using FerroShop.Data; 
using FerroShop.Models; 
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic; 
using System.Threading.Tasks; 

namespace FerroShop.Services
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> GetCategorias();
       
    }
}
