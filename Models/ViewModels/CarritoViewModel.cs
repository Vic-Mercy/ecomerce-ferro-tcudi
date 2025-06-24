using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerroShop.Models.ViewModels
{
    public class CarritoViewModel
    {
        public List<CarritoItemViewModel> Items { get; set; } = new List<CarritoItemViewModel>();
        public decimal Total { get; set; }

    }
}