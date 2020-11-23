using CoffeeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeAsp.ViewModel
{
    public class ProductIndexVM
    {
        public IEnumerable<Products> products { get; set; }
    }
}
